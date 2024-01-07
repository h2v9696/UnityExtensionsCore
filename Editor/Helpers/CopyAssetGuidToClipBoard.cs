using UnityEditor;
using UnityEngine;

namespace H2V.ExtensionsCore.Editor.Helpers
{
    /// <summary>
    /// Right click on any asset and copy its GUID to clipboard.
    /// </summary>
    public class CopyAssetGuidToClipBoard
    {
        [MenuItem("Assets/Copy Asset GUID %#x")]
        public static void CopyAssetGuidToClipboard()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var guid = AssetDatabase.AssetPathToGUID(path);
            GUIUtility.systemCopyBuffer = guid;
        }
    }
}
