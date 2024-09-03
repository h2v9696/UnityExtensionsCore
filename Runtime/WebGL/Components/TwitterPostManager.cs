using UnityEngine;
using System;
using System.Web;
using System.Xml.Linq;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
#if !UNITY_EDITOR && UNITY_WEBGL
	using System.Runtime.InteropServices;
#endif

namespace H2V.ExtensionsCore.WebGL.Components
{
    public struct TweetScreenShotParam
    {
        public string Message;
        public RectTransform RectTransform;
    }

    public class TwitterPostManager : MonoBehaviour
    {
        public static Action<string> PostMessage;
        public static Func<TweetScreenShotParam, UniTask> PostWithImage;


        [SerializeField] private string _imgurClientId;
        [SerializeField] private string _defaultMessage;

        private const string IMGUR_URL = "https://api.imgur.com/3/image.xml";

        private void OnEnable()
        {
            PostMessage += PostWithMessage;
            PostWithImage += PostWithScreenshot;
        }

        private void OnDisable()
        {
            PostMessage -= PostWithMessage;
            PostWithImage -= PostWithScreenshot;
        }

        private Texture2D CaptureScreenshotAsTexture(RectTransform rect)
        {
            if (rect == null)
            {
                return ScreenCapture.CaptureScreenshotAsTexture();
            }

            var corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            var bl = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]);
            var tl = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[1]);
            var tr = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[2]);

            var height = tl.y - bl.y;
            var width = tr.x - bl.x;

            Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
            Rect rex = new Rect(bl.x, bl.y, width, height);
            tex.ReadPixels(rex, 0, 0);
            tex.Apply();
            return tex;
        }

        private void PostWithMessage(string message)
        {
            WebGLUtils.TweetMessage(message);
        }

        /// <summary>
        /// If RectTransform is null the entire screen will be posted
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async UniTask PostWithScreenshot(TweetScreenShotParam param)
        {
            if (string.IsNullOrEmpty(_imgurClientId))
            {
                Debug.LogError($"Add your imgur client id to use this component");
            }
            var message = param.Message;
            message = string.IsNullOrEmpty(message) ? _defaultMessage : message;
            message = HttpUtility.UrlEncode(message);

            await UniTask.WaitForEndOfFrame(this);
            //Save the screenshot to disk
            Texture2D tex = CaptureScreenshotAsTexture(param.RectTransform);

            var wwwForm = new WWWForm();
            wwwForm.AddField("image", Convert.ToBase64String(tex.EncodeToJPG()));
            wwwForm.AddField("type", "base64");
            // Destroy texture to avoid memory leaks
            Destroy(tex);
            // Upload to Imgur
            using (var request = UnityWebRequest.Post(IMGUR_URL, wwwForm))
            {
                request.SetRequestHeader("AUTHORIZATION", $"Client-ID {_imgurClientId}");
                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Upload complete!");
                    XDocument xDoc = XDocument.Parse(request.downloadHandler.text);
                    var uri = xDoc.Element("data")?.Element("link")?.Value;

                    // Remove Ext
                    uri = uri?.Remove(uri.Length - 5, 5);
                    PostWithMessage($"{message}%0a{uri}");
                }
                else
                {
                    Debug.Log($"Upload error: {request.error}");
                }
            }
        }

        [Serializable]
        public class ImgurResponse
        {
            public Data data;
        }

        [Serializable]
        public class Data
        {
            public string link;
        }
    }
}