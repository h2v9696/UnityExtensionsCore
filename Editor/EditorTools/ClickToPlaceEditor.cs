using H2V.ExtensionsCore.Common;
using UnityEditor;
using UnityEngine;

namespace H2V.ExtensionsCore.Editor.EditorTools
{
    [CustomEditor(typeof(ClickToPlace))]
    public class ClickToPlaceEditor : UnityEditor.Editor
    {
        private ClickToPlace ClickToPlace => target as ClickToPlace;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Place at Mouse cursor") && !ClickToPlace.IsTargeting)
            {
                ClickToPlace.BeginTargeting();
                SceneView.duringSceneGui += DuringSceneGui;
            }
        }

        private void DuringSceneGui(SceneView sceneView)
        {
            var currentGUIEvent = Event.current;

            var mousePos = currentGUIEvent.mousePosition;
            var pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y * pixelsPerPoint;
            mousePos.x *= pixelsPerPoint;

            var pos = sceneView.camera.ScreenToWorldPoint(mousePos);

            ClickToPlace.UpdateTargeting(pos);

            switch (currentGUIEvent.type)
            {
                case EventType.MouseMove:
                    HandleUtility.Repaint();
                    break;
                case EventType.MouseDown:
                    if (currentGUIEvent.button == 0)
                    {
                        ClickToPlace.EndTargeting();
                        SceneView.duringSceneGui -= DuringSceneGui;
                        currentGUIEvent.Use();
                    }

                    break;
            }
        }
    }
}
