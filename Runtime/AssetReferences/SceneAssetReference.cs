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
                return ((SceneInstance) OperationHandle.Result).Scene;
            }
            var handler = LoadSceneAsync(loadMode, activateOnLoad, priority);
            
            await UniTask.WaitUntil(() => handler.IsDone);
            if (handler.Status == AsyncOperationStatus.Succeeded)
                return handler.Result.Scene;
            return default;
        }
    }
}