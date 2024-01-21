using H2V.ExtensionsCore.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;

namespace H2V.ExtensionsCore.Tests.ScriptableObjects
{
    [TestFixture, Category("Smoke Tests")]
    public class SerializableScriptableObjectTests
    {
        [Test]
        public void SerializableScriptableObjects_CreatedCorrectly()
        {
            var guids = AssetDatabase.FindAssets("t:SerializableScriptableObject");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var serializableScriptableObject
                    = AssetDatabase.LoadAssetAtPath<SerializableScriptableObject>(path);
                Assert.IsNotEmpty(serializableScriptableObject.Guid, $"{path} has no asset reference.");
            }
        }
    }
}