using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace CodeCatGames.HMPool.Runtime
{
    public sealed class PoolService
    {
        #region ReadonlyFields
        private readonly Dictionary<Type, object> _monoPools = new();
        private readonly Dictionary<Type, object> _purePools = new();
        private readonly PoolConfig _config;
        #endregion

        #region Fields
        private Transform _poolParent;
        #endregion

        #region Constructor
        public PoolService(PoolConfig config) => _config = config;
        #endregion

        #region Executes
        private void CreateDontDestroyOnLoadParent()
        {
            _poolParent = new GameObject("PoolParent").transform;
            
            Object.DontDestroyOnLoad(_poolParent);
        }
        private void InitializePools()
        {
            foreach (PoolDatum poolDatum in _config.PoolData)
            {
                if (poolDatum.IsMono)
                {
                    if (poolDatum.MonoPrefab)
                        CreatePool(poolDatum, typeof(MonoPool<>), _monoPools);
                }
                else
                {
                    if (poolDatum.ClassType != null)
                        CreatePool(poolDatum, typeof(PurePool<>), _purePools);
                }
            }
        }
        private void CreatePool(PoolDatum poolDatum, Type poolBaseType, Dictionary<Type, object> poolDictionary)
        {
            Type poolType = poolBaseType.MakeGenericType(poolDatum.ClassType);

            object pool = Activator.CreateInstance(poolType, poolDatum);
            
            poolDictionary[poolDatum.ClassType] = pool;
        }
        public T GetMono<T>() where T : MonoBehaviour, IPoolable
		{
			if (!_monoPools.TryGetValue(typeof(T), out object poolObj))
				throw new InvalidOperationException($"No pool found for type {typeof(T)}.");
			
			MonoPool<T> pool = poolObj as MonoPool<T>;
			
			Assert.IsNotNull(pool, "Pool object is null.");

			return pool.Get();

		}
		public T GetPure<T>() where T : class, IPoolable
		{
			if (!_purePools.TryGetValue(typeof(T), out object poolObj))
				throw new InvalidOperationException($"No pool found for type {typeof(T)}.");
			
			PurePool<T> purePool = poolObj as PurePool<T>;
			
			Assert.IsNotNull(purePool, "Pool object is null.");
			
			return purePool.Get();

		}
		public void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable
		{
			if (_monoPools.TryGetValue(typeof(T), out object poolObj))
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;
				
				Assert.IsNotNull(pool, "Pool object is null.");

				pool.Release(obj);
			}
			else
				Debug.LogError($"No pool found for type {typeof(T)}.");
		}
		public void ReleasePure<T>(T obj) where T : class, IPoolable
		{
			if (_purePools.TryGetValue(typeof(T), out object poolObj))
			{
				PurePool<T> purePool = poolObj as PurePool<T>;
				
				Assert.IsNotNull(purePool, "Pool object is null.");

				purePool.Release(obj);
			}
			else
				Debug.LogError($"No pool found for type {typeof(T)}.");
		}
		public void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable
		{
			if (_monoPools.TryGetValue(typeof(T), out object poolObj))
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;
				
				Assert.IsNotNull(pool, "Pool object is null.");

				pool.ReleaseAll();
			}
			else
				Debug.LogError($"No pool found for type {typeof(T)}.");
		}
		public void ReleaseAllPure<T>() where T : class, IPoolable
		{
			if (_purePools.TryGetValue(typeof(T), out object poolObj))
			{
				PurePool<T> purePool = poolObj as PurePool<T>;
				
				Assert.IsNotNull(purePool, "Pool object is null.");

				purePool.ReleaseAll();
			}
			else
				Debug.LogError($"No pool found for type {typeof(T)}.");
		}
		public void DestroyAllMono<T>() where T : MonoBehaviour, IPoolable
		{
			if (_monoPools.TryGetValue(typeof(T), out object poolObj))
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;
				
				Assert.IsNotNull(pool, "Pool object is null.");

				pool.DestroyAll();
			}
			else
				Debug.LogError($"No pool found for type {typeof(T)}.");
		}
		public void DestroyAllPure<T>() where T : class, IPoolable
		{
			if (_purePools.TryGetValue(typeof(T), out object poolObj))
			{
				PurePool<T> purePool = poolObj as PurePool<T>;
				
				Assert.IsNotNull(purePool, "Pool object is null.");

				purePool.DestroyAll();
			}
			else
				Debug.LogError($"No pool found for type {typeof(T)}.");
		}
        #endregion
    }
}