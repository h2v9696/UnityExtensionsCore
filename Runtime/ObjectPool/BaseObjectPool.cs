using UnityEngine;
using UnityEngine.Pool;

namespace H2V.ExtensionsCore.ObjectPool
{
    public abstract class BaseObjectPool<TItem> : MonoBehaviour where TItem : MonoBehaviour
    {
        [SerializeField] protected TItem _prefab;

        // Performane is better if this is false
        [Tooltip("If order is needed item will be set at first child when being getted")]
        [SerializeField] private bool _isOrderNeeded;

        private IObjectPool<TItem> _pool;

        private IObjectPool<TItem> Pool => _pool ??= new ObjectPool<TItem>(OnCreateItem, OnGetItem, OnReleaseItem,
            OnDestroyItem);

        protected virtual TItem OnCreateItem()
        {
            var item = Instantiate(_prefab, transform);
            return item;
        }

        protected virtual void OnGetItem(TItem item)
        {
            if (_isOrderNeeded)
                item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
        }

        protected virtual void OnReleaseItem(TItem item)
        {
            item.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyItem(TItem item)
            => Destroy(item.gameObject);

        public virtual TItem GetItem() => Pool.Get();

        public virtual void ReleaseItem(TItem item)
        {
            if (item == null || !item.gameObject.activeSelf)
                return;

            Pool.Release(item);
        }

        public virtual void ReleaseAll() => Pool.Clear();
    }
}