using System;
using UnityEngine;

namespace H2V.ExtensionsCore.AssetReferences
{
    [Serializable]
    public class AudioAssetReference : GeneralAssetReference<AudioClip>
    {
        public AudioAssetReference(string guid): base(guid)
        {}
    }
}