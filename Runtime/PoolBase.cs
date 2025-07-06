using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace HMPool.Runtime
{
    /// <summary>
    /// A base class for pooling objects of type T. This class manages the object pool, handles object creation, 
    /// retrieval, release, and destruction, and supports initialization of a set of default objects.
    /// </summary>
    /// <typeparam name="T">The type of objects that can be pooled. Must implement the IPoolable interface.</typeparam>
    public abstract class PoolBase<T> where T : class, IPoolable
    {
        #region ReadonlyFields
        protected readonly PoolDatum PoolDatum;
        private readonly ObjectPool<T> _pool;
        private readonly List<T> _initialObjects = new();
        private readonly HashSet<T> _activeObjects = new();
        private readonly HashSet<T> _releasedObjects = new();
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PoolBase{T}"/> class with the specified pool configuration.
        /// </summary>
        /// <param name="poolDatum">The configuration data for the pool, including initial size, default capacity, and maximum size.</param>
        public PoolBase(PoolDatum poolDatum)
        {
            PoolDatum = poolDatum;
            
            _pool = new ObjectPool<T>(CreateObject, GetFromPool, ReturnToPool, DestroyObject, true,
                poolDatum.DefaultCapacity, poolDatum.MaximumSize);
        }
        #endregion

        #region Executes
        protected abstract T CreateObject();
        protected abstract void GetFromPool(T obj);
        protected abstract void ReturnToPool(T obj);
        protected abstract void DestroyObject(T obj);
        protected void InstantiateDefaultObjects()
        {
            for (int i = 0; i < PoolDatum.InitialSize; i++)
            {
                T obj = _pool.Get();
                
                _initialObjects.Add(obj);
            }
            
            _initialObjects.ForEach(Release);
            
            _initialObjects.Clear();
        }
        
        /// <summary>
        /// Retrieves an object from the pool and adds it to the active objects list.
        /// </summary>
        /// <returns>A pooled object of type T.</returns>
        internal T Get()
        {
            T poolable = _pool.Get();
            
            _activeObjects.Add(poolable);

            if (_releasedObjects.Contains(poolable))
                _releasedObjects.Remove(poolable);
            
            return poolable;
        }
        
        /// <summary>
        /// Releases the specified object back into the pool and adds it to the released objects list.
        /// </summary>
        /// <param name="obj">The object to be released back into the pool.</param>
        internal void Release(T obj)
        {
            if (_releasedObjects.Contains(obj))
                return;
            
            _activeObjects.Remove(obj);
            
            _pool.Release(obj);
            
            _releasedObjects.Add(obj);
        }
        
        /// <summary>
        /// Destroys the specified object and removes it from the active and released objects lists.
        /// </summary>
        /// <param name="obj">The object to be destroyed.</param>
        internal void Destroy(T obj)
        {
            if (_activeObjects.Contains(obj))
                _activeObjects.Remove(obj);
    
            if (_releasedObjects.Contains(obj))
                _releasedObjects.Remove(obj);

            DestroyObject(obj);
        }
        
        /// <summary>
        /// Releases all active objects back into the pool.
        /// </summary>
        internal void ReleaseAll()
        {
            HashSet<T> activeObjects = new(_activeObjects);
            
            activeObjects.ToList().ForEach(Release);
        }
        
        /// <summary>
        /// Destroys all objects in the pool, clearing both active and released objects lists, and clearing the pool.
        /// </summary>
        internal void DestroyAll()
        {
            _activeObjects.Clear();
            _releasedObjects.Clear();
            
            _pool.Clear();
        }
        #endregion
    }
}