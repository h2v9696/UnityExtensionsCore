using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace H2V.ExtensionsCore.Editor.Helpers
{
    public static class AddressableExtensions
    {
        public static void SetObjectToAddressableGroup(string objectGuid, string groupName, bool isSimplifyName = false)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
 
            if (!settings) return;
            
            var group = settings.FindGroup(groupName);
            if (!group)
                group = settings.CreateGroup(groupName, false, false, true, null,
                    typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

            var assetEntry = settings.CreateOrMoveEntry(objectGuid, group, false, false);
            if (isSimplifyName)
            {
                var assetName = System.IO.Path.GetFileNameWithoutExtension(assetEntry.AssetPath);
                assetEntry.SetAddress(assetName);
            }
            
            var entriesAdded = new List<AddressableAssetEntry> {assetEntry};

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved,
                entriesAdded,false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved,
                entriesAdded, true, false);
        } 
        
        public static void SetObjectToAddressableGroup(this Object obj, string groupName)
        {
            var assetpath = AssetDatabase.GetAssetPath(obj);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);

            SetObjectToAddressableGroup(guid, groupName);
        } 
    }
}