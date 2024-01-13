using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ScriptableObject), true), CanEditMultipleObjects]
public class ScriptableObjectSelect : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select", EditorStyles.miniButton, GUILayout.MaxWidth(48)))
                EditorGUIUtility.PingObject(this.target);
        }

        GUILayout.Space(12);
        base.OnInspectorGUI();
    }
}
