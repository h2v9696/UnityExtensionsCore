using System;
using UnityEngine;

namespace H2V.ExtensionsCore.Events.ScriptableObjects
{
    public abstract class GenericReturnEventChannelSO<T, TResult> : ScriptableObject
    {
        public event Func<T, TResult> EventRaised;

        public virtual TResult RaiseEvent(T obj) => OnRaiseEvent(obj);

        protected virtual TResult OnRaiseEvent(T obj)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"OnRaiseEvent:: No listeners for event {name}.");
                return default(TResult);
            }

            return EventRaised.Invoke(obj);
        }
    }
}