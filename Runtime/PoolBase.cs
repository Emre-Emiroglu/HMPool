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
        private readonly List<T> _initialObjectList = new();
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

        #region Core
        protected abstract T CreateObject();
        protected abstract void GetFromPool(T obj);
        protected abstract void ReturnToPool(T obj);
        protected abstract void DestroyObject(T obj);
        #endregion

        #region Executes
        protected void InstantiateDefaultObjects()
        {
            for (int i = 0; i < PoolDatum.InitialSize; i++)
            {
                T obj = _pool.Get();
                
                _initialObjectList.Add(obj);
            }
            
            _initialObjectList.ForEach(Release);
            
            _initialObjectList.Clear();
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
        internal void ReleaseAll()
        {
            HashSet<T> activeObjects = new(_activeObjects);
            
            activeObjects.ToList().ForEach(Release);
        }
        internal void DestroyAll() => _pool.Clear();
        #endregion
    }
}