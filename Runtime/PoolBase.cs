using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace CodeCatGames.HMPool.Runtime
{
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
        internal T Get()
        {
            T poolable = _pool.Get();
            
            _activeObjects.Add(poolable);

            if (_releasedObjects.Contains(poolable))
                _releasedObjects.Remove(poolable);
            
            return poolable;
        }
        internal void Release(T obj)
        {
            if (_releasedObjects.Contains(obj))
                return;
            
            _activeObjects.Remove(obj);
            
            _pool.Release(obj);
            
            _releasedObjects.Add(obj);
        }
        internal void Destroy(T obj)
        {
            if (_activeObjects.Contains(obj))
                _activeObjects.Remove(obj);
    
            if (_releasedObjects.Contains(obj))
                _releasedObjects.Remove(obj);

            DestroyObject(obj);
        }
        internal void ReleaseAll()
        {
            HashSet<T> activeObjects = new(_activeObjects);
            
            activeObjects.ToList().ForEach(Release);
        }
        internal void DestroyAll()
        {
            _activeObjects.Clear();
            _releasedObjects.Clear();
            
            _pool.Clear();
        }
        #endregion
    }
}