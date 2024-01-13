using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FuzzyFinder
{
    static class Styles
    {
        internal static Vector2 SIZE;
        internal static int LABEL_SIZE = 160;

        static Styles()
        {
            SIZE = new Vector2(720, 84);
        }
    }

    public class FuzzyFinder
	{
        static FindPopupWindow instance = null;

#if UNITY_EDITOR_WIN
        [UnityEditor.MenuItem("Tools/Fuzzy file finder %`")]
#elif UNITY_EDITOR_OSX
        [UnityEditor.MenuItem("Tools/Fuzzy file finder ^`")]
#endif
        public static void DoSomething()
		{
            //Folder.ShowFolderContents("Assets/QuickFindFiles");

            var midScreen = EditorGUIUtility.ScreenToGUIPoint(new Vector2(Screen.width / 2, Screen.height / 2));
            var rect = new Rect(midScreen, Styles.SIZE);
            rect.position += Styles.SIZE / 4;
            rect.y -= 120;

            try
            {
                if (instance == null)
                {
                    instance = new FindPopupWindow();
                    instance.onClosed += ClearInstance;
                    instance.onSelected += FindResolve;
                    instance.onOpenned += OpenResolve;
                }
                PopupWindow.Show(rect, instance);
            }
            catch { }
        }

        static void ClearInstance()
        {
            instance = null;
        }

        static void FindResolve(string path, UnityEngine.Object obj)
        {
            if (System.IO.Directory.Exists(path))
            {
                Folder.ShowFolderContents(path);
            }
            else
            {
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
        }

        static void OpenResolve(string path, UnityEngine.Object obj)
        {
            if (System.IO.Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
            }
            else
                AssetDatabase.OpenAsset(obj);
        }

    }

    class FindPopupWindow : PopupWindowContent
    {

        struct SearchMatch
        {
            public string path;
            public int score;
        }

        AutocompleteSearchField autocompleteSearchField;
        List<string> assetPaths;
        List<SearchMatch> matches;

        public System.Action onClosed;
        public System.Action<string, UnityEngine.Object> onSelected;
        public System.Action<string, UnityEngine.Object> onOpenned;

        public FindPopupWindow()
        {
            autocompleteSearchField = new AutocompleteSearchField();
            autocompleteSearchField.onInputChanged = OnInputChanged;
            autocompleteSearchField.onConfirm = OnConfirm;

            assetPaths = new List<string>(AssetDatabase.GetAllAssetPaths());
            assetPaths.RemoveAll((s) => s.StartsWith("Assets") == false);
            assetPaths.Sort((s1, s2) => s1.CompareTo(s2));
            matches = new List<SearchMatch>(assetPaths.Count);
        }

        public override void OnClose()
        {
            onClosed?.Invoke();
        }

        public override Vector2 GetWindowSize()
        {
            var s = Styles.SIZE;
            s.y += autocompleteSearchField.GetExtendedHeight();
            return s;
        }

        public override void OnGUI(Rect rect)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                this.editorWindow.Close();
                return;
            }

            rect.width -= 12;
            rect.x += 6;
            rect.y += 4;
            GUILayout.BeginArea(rect);
            GUILayout.Label("Fuzzy file finder", EditorStyles.miniBoldLabel);
            GUILayout.Label("Hold shift when selected to show containing folder. Hold CMD when selected to open the asset.", EditorStyles.miniLabel);
            GUILayout.Label(" ", EditorStyles.miniLabel);
            autocompleteSearchField.OnGUI();
            GUILayout.EndArea();
        }

        void OnInputChanged(string searchString)
        {
            autocompleteSearchField.ClearResults();
            matches.Clear();
            if (!string.IsNullOrEmpty(searchString))
            {
                int score = 0;
                int count = 0;

                foreach (var assetPath in assetPaths)
                {
                    if (FuzzyMatcher.FuzzyMatch(assetPath, searchString, out score))
                    {
                        matches.Add(new SearchMatch()
                        {
                            path = assetPath,
                            score = score
                        });
                        count++;
                        if (count > 90) break;
                    }
                }

                matches.Sort((m1, m2) => m2.score.CompareTo(m1.score));
                for (var i = 0; i < Mathf.Min(matches.Count, autocompleteSearchField.maxResults); i++)
                    autocompleteSearchField.AddResult(matches[i].path);
            }
        }

        void OnConfirm(string result)
        {
            if (Event.current.shift)
            {
                var i = result.LastIndexOf('/');
                result = result.Substring(0, i);
            }

            var obj = AssetDatabase.LoadMainAssetAtPath(autocompleteSearchField.searchString);

            if (Event.current.command)
            {
                this.onOpenned?.Invoke(result, obj);

            } else
            {
                this.onSelected?.Invoke(result, obj);
            }

            this.editorWindow.Close();

        }
    }
}