using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using H2V.ExtensionsCore.WebGL.Components;

namespace H2V.ExtensionsCore.WebGL.UI
{
	public class UIPostArea : MonoBehaviour
	{
		[SerializeField, Tooltip("Crop screenshot")]
		private RectTransform _captureRect;

		[SerializeField, Tooltip("Object that hidden in screenshot")] 
		private GameObject[] _cullingObjects;

		[SerializeField, Tooltip("Delay screenshot to prevent texture not done yet")] 
		private float _delayScreenshot = 1;

		[field: SerializeField] 
		public string _message { get; set; }

		private void OnValidate()
		{
			if (_captureRect != null) return;
			_captureRect = GetComponent<RectTransform>();
		}

		public void SetActiveCullingObjects(bool value)
		{
			foreach (GameObject obj in _cullingObjects)
			{
				obj.SetActive(value);
			}
		}

        public void TweetScreenShot()
        {
            TweetScreenShotAsync().Forget();
        }

		public async UniTask TweetScreenShotAsync()
		{
			SetActiveCullingObjects(false);
			// Delay to prevent mouse effect still showing
			await UniTask.Delay(TimeSpan.FromSeconds(_delayScreenshot));

			await TwitterPostManager.PostWithImage(new()
            {
                Message = _message,
                RectTransform = _captureRect
            });

            SetActiveCullingObjects(true);
		}
	}
}
