using UnityEngine;

namespace H2V.ExtensionsCore.Common
{
    /// <summary>
    /// Disable other log than exception in release build
    /// </summary>
    public class DisableLog : MonoBehaviour
    {
        private void Awake() 
        {
#if !(DEVELOPMENT_BUILD || UNITY_EDITOR)
            Debug.unityLogger.filterLogType = LogType.Exception;
#endif
        }
    }
}