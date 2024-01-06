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
    public class MockSO : ScriptableObject { }

    [TestFixture, Category("Unit Tests")]
    public class ScriptableObjectAssetReferenceTests
    {
        private ScriptableObjectAssetReference<MockSO> _assetReference;
        private string _assetGuid;
        private string _assetPath;

        [SetUp]
        public void SetUp()
        {
            var mockSO = ScriptableObject.CreateInstance<MockSO>();

            // create Temp folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder("Assets/Temp"))
            {
                AssetDatabase.CreateFolder("Assets", "Temp");
            }

            _assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Temp/MockSO.asset");
            AssetDatabase.CreateAsset(mockSO, _assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _assetGuid = AssetDatabase.AssetPathToGUID(_assetPath);
            _assetReference = new ScriptableObjectAssetReference<MockSO>(_assetGuid);
        }

        [Test]
        public void ValidateAsset_WithAssetPath_ReturnsTrue()
        {
            Assert.IsTrue(_assetReference.ValidateAsset(_assetPath));
        }

        [Test]
        public void ValidateAssets_WithObject_ShouldReturnTrue()
        {
            var scriptableObjectAssetReference = new ScriptableObjectAssetReference<MockSO>("0");

            var mockSO = ScriptableObject.CreateInstance<MockSO>();

            Assert.True(scriptableObjectAssetReference.ValidateAsset(mockSO));
        }

        [Test]
        public void ValidateAssets_WithEmptyPath_ShouldReturnFalse()
        {
            var scriptableObjectAssetReference = new ScriptableObjectAssetReference<MockSO>("0");

            Assert.False(scriptableObjectAssetReference.ValidateAsset(string.Empty));
        }

        [Test]
        public void ValidateAssets_WithGameObject_ShouldReturnFalse()
        {
            var scriptableObjectAssetReference = new ScriptableObjectAssetReference<MockSO>("0");

            var gameObject = new GameObject();

            Assert.False(scriptableObjectAssetReference.ValidateAsset(gameObject));
        }

        [UnityTest]
        public IEnumerator TryLoadAsset_ValidScene_SceneShouldLoaded()
        {
            const string TEST_GROUP = "TestGroup"; 
            AddressableExtensions.SetObjectToAddressableGroup(_assetGuid, TEST_GROUP);
            yield return UniTask.ToCoroutine(async () =>
            {
                var asset = await _assetReference.TryLoadAsset();
                Assert.IsNotNull(asset);
                Assert.True(asset.GetType() == typeof(MockSO));
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
                var asset = await _assetReference.TryLoadAsset();
                Assert.IsNull(asset);
            });
        }

        [TearDown]
        public void TearDown()
        {
            AssetDatabase.DeleteAsset("Assets/Temp");
        }
    }
}