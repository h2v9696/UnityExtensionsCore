using System;
using UnityEngine;

namespace H2V.ExtensionsCore.Events.ScriptableObjects
{
    public abstract class GenericEventChannelSO<T> : ScriptableObject
    {
        public event Action<T> EventRaised;

        public virtual void RaiseEvent(T obj) => OnRaiseEvent(obj);

        protected virtual void OnRaiseEvent(T obj)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"OnRaiseEvent:: No listeners for event {name}.");
                return;
            }

            EventRaised.Invoke(obj);
        }
    }
}