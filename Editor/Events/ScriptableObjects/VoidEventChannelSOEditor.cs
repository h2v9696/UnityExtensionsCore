using UnityEditor;
using UnityEngine;

namespace H2V.ExtensionsCore.Events.ScriptableObjects
{
    [CustomEditor(typeof(VoidEventChannelSO), true)]
    public class VoidEventChannelSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var eventSO = target as VoidEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
                eventSO.RaiseEvent();
        }
    }
}