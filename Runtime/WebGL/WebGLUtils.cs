using UnityEngine;
#if !UNITY_EDITOR && UNITY_WEBGL
    using System.Runtime.InteropServices;
#endif

namespace H2V.ExtensionsCore.WebGL
{
    public static class WebGLUtils
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void CopyToClipboard(string text);
        
        [DllImport("__Internal")]
        private static extern string TweetFromUnity(string rawMessage);
        
        [DllImport("__Internal")]
        private static extern string SuperRefreshPage();
#endif

        public static void CopyTextToClipboard(string text)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            CopyToClipboard(text);
#else
            GUIUtility.systemCopyBuffer = text;
#endif
        }

        public static void TweetMessage(string message)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            TweetFromUnity(message);
#else
            Debug.LogWarning($"This only work in WebGL build");
#endif
        }

        public static void RefreshAndClearCache()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            SuperRefreshPage();
#else
            Debug.LogWarning($"This only work in WebGL build");
#endif
        }
    }
}