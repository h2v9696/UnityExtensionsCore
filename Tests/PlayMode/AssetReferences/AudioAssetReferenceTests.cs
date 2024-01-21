using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using H2V.ExtensionsCore.Editor.Helpers;
using UnityEditor;
using UnityEditor.AddressableAssets;
using H2V.ExtensionsCore.AssetReferences;

namespace H2V.ExtensionsCore.Tests.AssetReferences
{
    [TestFixture, Category("Unit Tests")]
    public class AudioAssetReferenceTests
    {
        private AudioAssetReference _assetReference;
        private const string AUDIO_CLIP_GUID = "6f55f5dca03006d449839e1e968c7c0a";
        private string _assetPath;

        [SetUp]
        public void SetUp()
        {
            _assetReference = new AudioAssetReference(AUDIO_CLIP_GUID);
            _assetPath = AssetDatabase.GUIDToAssetPath(AUDIO_CLIP_GUID);
        }

        [Test]
        public void ValidateAsset_WithAssetPath_ReturnsTrue()
        {
            Assert.IsTrue(_assetReference.ValidateAsset(_assetPath));
        }

        [Test]
        public void ValidateAssets_WithObject_ShouldReturnTrue()
        {
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(_assetPath);
            Assert.True(_assetReference.ValidateAsset(audioClip));
        }

        [Test]
        public void ValidateAssets_WithEmptyPath_ShouldReturnFalse()
        {
            Assert.False(_assetReference.ValidateAsset(string.Empty));
        }

        [Test]
        public void ValidateAssets_WithGameObject_ShouldReturnFalse()
        {
            var gameObject = new GameObject();
            Assert.False(_assetReference.ValidateAsset(gameObject));
        }

        [UnityTest]
        public IEnumerator TryLoadAsset_ValidAudioClip_ShouldLoaded()
        {
            const string TEST_GROUP = "TestGroup"; 
            AddressableExtensions.SetObjectToAddressableGroup(AUDIO_CLIP_GUID, TEST_GROUP);
            yield return UniTask.ToCoroutine(async () =>
            {
                var asset = await _assetReference.TryLoadAsset();
                Assert.IsNotNull(asset);
                Assert.True(asset.GetType() == typeof(AudioClip));
            });

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.RemoveGroup(settings.FindGroup(TEST_GROUP));
        }

        [UnityTest]
        public IEnumerator TryLoadAsset_InvalidAudioClip_ShouldLoadFail()
        {
            LogAssert.ignoreFailingMessages = true;
            yield return UniTask.ToCoroutine(async () =>
            {
                var asset = await _assetReference.TryLoadAsset();
                Assert.IsNull(asset);
            });
        }
    }
}