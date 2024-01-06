using UnityEngine;

namespace H2V.ExtensionsCore.Events.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StringEvent", menuName = "H2V/Extensions Core/Events/String Event Channel")]
    public class StringEventChannelSO : GenericEventChannelSO<string> { }
}