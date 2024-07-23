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
    public class GeneralAssetReference<T> : AssetReferenceT<T> where T : Object
    {
        public new AsyncOperationHandle<T> OperationHandle { get; private set; }
        public GeneralAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj)
        {
            return obj as T != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var so = AssetDatabase.LoadAssetAtPath<T>(path);
            return so != null;
#else
            return false;
#endif
        }

        public async UniTask<T> TryLoadAsset()
        {
            if (OperationHandle.IsValid() && OperationHandle.IsDone)
            {
                return OperationHandle.Result;
            }

            OperationHandle = Addressables.LoadAssetAsync<T>(this);
            
            await UniTask.WaitUntil(() => OperationHandle.IsDone);
            if (OperationHandle.Status == AsyncOperationStatus.Succeeded)
                return OperationHandle.Result;
            return default;
        }

        public override void ReleaseAsset()
        {
            if (!OperationHandle.IsValid()) return;
            Addressables.Release(OperationHandle);
        }
    }
}