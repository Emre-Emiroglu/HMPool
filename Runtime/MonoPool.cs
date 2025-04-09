using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    /// <summary>
    /// A pool that manages MonoBehaviour-based objects, providing methods to create, retrieve, return, and destroy them.
    /// </summary>
    /// <typeparam name="T">The type of MonoBehaviour that is poolable and implements the <see cref="IPoolable"/> interface.</typeparam>
    public sealed class MonoPool<T> : PoolBase<T> where T : MonoBehaviour, IPoolable
    {
        #region ReadonlyFields
        private readonly Transform _parent;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoPool{T}"/> class with the specified pool configuration and parent transform.
        /// </summary>
        /// <param name="poolDatum">The configuration data for the pool, including initial size, default capacity, and maximum size.</param>
        /// <param name="parent">The parent transform under which the pooled MonoBehaviour objects will be instantiated.</param>
        public MonoPool(PoolDatum poolDatum, Transform parent) : base(poolDatum)
        {
            _parent = parent;

            InstantiateDefaultObjects();
        }
        #endregion

        #region Core
        protected override T CreateObject()
        {
            T obj = Object.Instantiate(PoolDatum.MonoPrefab, _parent).GetComponent<T>();
            
            obj.OnCreated();
			
            return obj;
        }
        protected override void GetFromPool(T obj)
        {
            obj.OnGetFromPool();
            
            obj.gameObject.SetActive(true);
        }
        protected override void ReturnToPool(T obj)
        {
            obj.OnReturnToPool();
            
            obj.gameObject.SetActive(false);
        }
        protected override void DestroyObject(T obj)
        {
            obj.OnDestroyed();
            
            Object.Destroy(obj.gameObject);
        }
        #endregion
    }
}