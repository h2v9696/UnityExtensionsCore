using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace H2V.ExtensionsCore.Common
{
    /// <summary>
    /// Add this component to an object in scene to click and place it
    /// Only work in 2D mode
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("H2V/Tools/Click to Place")]
    public class ClickToPlace : MonoBehaviour
    {
        private Vector2 _targetPosition;

        public bool IsTargeting { get; private set; }

        private void OnDrawGizmos()
        {
            if (!IsTargeting) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_targetPosition, Vector3.one);
        }

        public void BeginTargeting()
        {
            IsTargeting = true;
            _targetPosition = transform.position;
        }

        public void UpdateTargeting(Vector2 spawnPosition)
        {
            _targetPosition = spawnPosition;
        }

        public void EndTargeting()
        {
            IsTargeting = false;
#if UNITY_EDITOR
            Undo.RecordObject(transform, $"{gameObject.name} moved by ClickToPlaceHelper");
#endif
            transform.position = _targetPosition;
        }
    }
}