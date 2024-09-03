using H2V.ExtensionsCore.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace H2V.ExtensionsCore.Events.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InputEventChannelSO", menuName = "H2V/Extensions Core/Events/Input Event Channel")]
    public class InputEventChannelSO : GenericEventChannelSO<InputAction.CallbackContext>
    {
    }
}