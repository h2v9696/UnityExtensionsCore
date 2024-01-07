using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace H2V.ExtensionsCore.Editor.Helpers
{
    public static class AssetFinder
    {
        /// <summary>
        /// This method is used to get all assets from a type.
        /// </summary>
        /// <param name="additionOption">For example add nbame to find with name</param>
        /// <param name="path">Find within path. Default: Assets folder</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> FindAssetsWithType<T>(string additionOption, string path = "Assets") where T : Object
        {
            List<T> assets = new();

            foreach (string guid in AssetDatabase.FindAssets($"t:{typeof(T).Name} {additionOption}", new[] {path}))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetsAtPath = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                assets.Add(assetsAtPath);
            }

            return assets;
        }

        /// <summary>
        /// This method is used to get an first asset with its name.
        /// </summary>
        /// <param name="assetName">The name asset want to find </param>
        /// <typeparam name="T">The type asset want to find</typeparam>
        /// <returns></returns>
        public static T FindAssetsWithName<T>(string assetName) where T : Object
        {
            var allAssets = FindAssetsWithType<T>(assetName);

            return allAssets.FirstOrDefault();
        }

        public static T FindAssetWithNameInPath<T>(string assetName, string path) where T : Object
        {
            var allAssets = FindAssetsWithType<T>(assetName, path);

            return allAssets.FirstOrDefault() ;
        }
    }
}