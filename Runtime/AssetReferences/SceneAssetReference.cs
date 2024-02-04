using System;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace H2V.ExtensionsCore.AssetReferences
{
    [Serializable]
    public class SceneAssetReference : AssetReference
    {
        public new AsyncOperationHandle<SceneInstance> OperationHandle { get; private set; }
        public SceneAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(string path)
        {
            return path.EndsWith(".unity");
        }

        public async UniTask<Scene> TryLoadScene(LoadSceneMode loadMode, 
            bool activateOnLoad = true, int priority = 0)
        {
            if (OperationHandle.IsValid() && OperationHandle.IsDone)
            {
                return OperationHandle.Result.Scene;
            }
            // I saving this and handle it myself because AssetReference.LoadSceneAsync() error when load twice
            OperationHandle = Addressables.LoadSceneAsync(this, loadMode, activateOnLoad, priority);
            
            await UniTask.WaitUntil(() => OperationHandle.IsDone);
            if (OperationHandle.Status == AsyncOperationStatus.Succeeded)
                return OperationHandle.Result.Scene;
            return default;
        }

        public override void ReleaseAsset()
        {
            if (!OperationHandle.IsValid()) return;
            Addressables.Release(OperationHandle);
        }
    }
}