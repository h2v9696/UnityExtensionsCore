using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace H2V.ExtensionsCore.AssetReferences
{
    [Serializable]
    public class AudioAssetReference : AssetReferenceT<AudioClip>
    {
        public AudioAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj)
        {
            return obj as AudioClip != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var so = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            return so != null;
#else
            return false;
#endif
        }

        public async UniTask<AudioClip> TryLoadAsset()
        {
            var handler = LoadAssetAsync<AudioClip>();
            if (OperationHandle.IsValid() && OperationHandle.IsDone)
            {
                return (AudioClip) OperationHandle.Result;
            }
            
            await UniTask.WaitUntil(() => handler.IsDone);
            if (handler.Status == AsyncOperationStatus.Succeeded)
                return handler.Result;
            return default;
        }
    }
}