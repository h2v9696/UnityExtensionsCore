using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using H2V.ExtensionsCore.Editor.Helpers;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using H2V.ExtensionsCore.AssetReferences;

namespace H2V.ExtensionsCore.Tests.AssetReferences
{
    [TestFixture, Category("Unit Tests")]
    public class SceneAssetReferenceTests
    {
        private SceneAssetReference _sceneAssetReference;
        private string _validTestSceneGuid = "8ed1086e7e029914db0e85ddcaf75309";

        [SetUp]
        public void Setup()
        {
            _sceneAssetReference = new SceneAssetReference(_validTestSceneGuid);
        }

        [Test]
        public void ValidateAsset_ValidSceneAsset_ShouldTrue()
        {
            var assetName = AssetDatabase.GUIDToAssetPath(_validTestSceneGuid);
            Assert.IsTrue(_sceneAssetReference.ValidateAsset(assetName));
        }

        [Test]
        public void ValidateAsset_InvalidSceneAsset_ShouldFalse()
        {
            Assert.IsFalse(_sceneAssetReference.ValidateAsset(string.Empty));
        }

        [UnityTest]
        public IEnumerator TryLoadAsset_ValidScene_SceneShouldLoaded()
        {
            const string TEST_GROUP = "TestGroup"; 
            AddressableExtensions.SetObjectToAddressableGroup(_validTestSceneGuid, TEST_GROUP);
            yield return UniTask.ToCoroutine(async () =>
            {
                var scene = await _sceneAssetReference.TryLoadAsset(LoadSceneMode.Additive);
                Assert.IsNotNull(scene);
            });

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.RemoveGroup(settings.FindGroup(TEST_GROUP));
        }

        [UnityTest]
        public IEnumerator TryLoadAsset_InvalidScene_ShouldLoadFail()
        {
            LogAssert.ignoreFailingMessages = true;
            yield return UniTask.ToCoroutine(async () =>
            {
                var scene = await _sceneAssetReference.TryLoadAsset(LoadSceneMode.Additive);
                Assert.IsFalse(scene.IsValid());
            });
        }
    }
}