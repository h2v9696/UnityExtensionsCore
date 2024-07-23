using System;
using UnityEngine;

namespace H2V.ExtensionsCore.AssetReferences
{
    [Serializable]
    public class SpriteAssetReference : GeneralAssetReference<Sprite>
    {
        public SpriteAssetReference(string guid): base(guid)
        {}
    }
}