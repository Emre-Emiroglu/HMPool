using System;

namespace HMPool.Runtime
{
    /// <summary>
    /// A pool that manages class-based objects (non-MonoBehaviour), providing methods to create, retrieve, return, and destroy them.
    /// </summary>
    /// <typeparam name="T">The type of class that is poolable and implements the <see cref="IPoolable"/> interface.</typeparam>
    public sealed class PurePool<T> : PoolBase<T> where T : class, IPoolable
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PurePool{T}"/> class with the specified pool configuration.
        /// </summary>
        /// <param name="poolDatum">The configuration data for the pool, including initial size, default capacity, and maximum size.</param>
        public PurePool(PoolDatum poolDatum) : base(poolDatum) => InstantiateDefaultObjects();
        #endregion

        #region Core
        protected override T CreateObject()
        {
            T obj = Activator.CreateInstance<T>();
            
            obj.OnCreated();
			
            return obj;
        }
        protected override void GetFromPool(T obj) => obj.OnGetFromPool();
        protected override void ReturnToPool(T obj) => obj.OnReturnToPool();
        protected override void DestroyObject(T obj) => obj.OnDestroyed();
        #endregion
    }
}