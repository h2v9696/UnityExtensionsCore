using System;
using System.Collections.Generic;
using UnityEngine;

namespace H2V.ExtensionsCore.Common
{
    /// <summary>
    /// This class is used to cache components to avoid calling GetComponent/TryGetComponent multiple times.
    /// </summary>
    [AddComponentMenu("H2V/Tools/Cachable Component Getter")]
    public class CacheableComponentGetter : MonoBehaviour
    {
        private readonly Dictionary<Type, object> _cachedComponents = new();

        public new T GetComponent<T>()
        {
            if (TryGetFromCache<T>(out var component)) return component;
            component = base.GetComponent<T>();
            if (component != null) _cachedComponents.Add(typeof(T), component);
            return component;
        }

        public new T GetComponentInChildren<T>(bool includeInactive = false)
        {
            if (TryGetFromCache<T>(out var component)) return component;
            component = base.GetComponentInChildren<T>(includeInactive);
            if (component != null) _cachedComponents.Add(typeof(T), component);
            return component;
        }

        public new bool TryGetComponent<T>(out T component)
        {
            if (TryGetFromCache(out component)) return true;
            var result = base.TryGetComponent(out component);
            if (result) _cachedComponents.Add(typeof(T), component);
            return result;
        }

        private bool TryGetFromCache<T>(out T component)
        {
            component = default;
            var type = typeof(T);
            if (!_cachedComponents.TryGetValue(type, out var cachedComponent)) return false;
            component = (T)cachedComponent;
            return true;
        }
    }
}