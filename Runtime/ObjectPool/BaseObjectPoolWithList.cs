using System.Collections.Generic;
using UnityEngine;

namespace H2V.ExtensionsCore.ObjectPool
{
    // Has a list with current items being getted
    public abstract class BaseObjectPoolWithList<TItem> : BaseObjectPool<TItem> 
        where TItem : MonoBehaviour
    {
        protected List<TItem> _items = new();
        public List<TItem> Items => _items;

        protected override void OnGetItem(TItem item)
        {
            base.OnGetItem(item);
            _items.Add(item);
        }

        protected override void OnReleaseItem(TItem item)
        {
            base.OnReleaseItem(item);
            _items.Remove(item);
        }

        protected override void OnDestroyItem(TItem item)
        {
            _items.Remove(item);
            base.OnDestroyItem(item);
        }

        public override void ReleaseAll()
        {
            base.ReleaseAll();
            while (_items.Count > 0)
            {
                ReleaseItem(_items[0]);
            }
        }
    }
}