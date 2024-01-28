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
    public class ScriptableObjectAssetReference<TScriptableObject> : AssetReferenceT<TScriptableObject>
        where TScriptableObject : ScriptableObject
    {
        public new AsyncOperationHandle<TScriptableObject> OperationHandle { get; private set; }
        public ScriptableObjectAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj)
        {
            return obj as TScriptableObject != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var so = AssetDatabase.LoadAssetAtPath<TScriptableObject>(path);
            return so != null;
#else
            return false;
#endif
        }

        public async UniTask<TScriptableObject> TryLoadAsset()
        {
            if (OperationHandle.IsValid() && OperationHandle.IsDone)
            {
                return OperationHandle.Result;
            }

            OperationHandle = Addressables.LoadAssetAsync<TScriptableObject>(this);
            
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