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
        public void Initialize()
        {
	        CreateDontDestroyOnLoadParent();
	        InitializePools();
        }
        private void CreateDontDestroyOnLoadParent()
        {
            _poolParent = new GameObject("PoolParent").transform;
            
            Object.DontDestroyOnLoad(_poolParent);
        }
        private void InitializePools()
        {
	        foreach (PoolDatum poolDatum in _config.PoolData)
		        CreatePool(poolDatum, poolDatum.IsMono ? typeof(MonoPool<>) : typeof(PurePool<>),
			        poolDatum.IsMono ? _monoPools : _purePools);
        }
        private void CreatePool(PoolDatum poolDatum, Type poolBaseType, Dictionary<Type, object> poolDictionary,
	        Transform parent = null)
        {
	        Type poolType = poolBaseType.MakeGenericType(poolDatum.ClassType);

	        object[] parameters = parent != null ? new object[] { poolDatum, parent } : new object[] { poolDatum };

	        object pool = Activator.CreateInstance(poolType, parameters);
    
	        poolDictionary[poolDatum.ClassType] = pool;
        }
        public T GetMono<T>() where T : MonoBehaviour, IPoolable
		{
			if (!_monoPools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.ThrowNoPoolFoundException<T>();
			
			MonoPool<T> pool = poolObj as MonoPool<T>;
			
			Assert.IsNotNull(pool, "Pool object is null.");

			return pool.Get();
		}
		public T GetPure<T>() where T : class, IPoolable
		{
			if (!_purePools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.ThrowNoPoolFoundException<T>();
			
			PurePool<T> pool = poolObj as PurePool<T>;
			
			Assert.IsNotNull(pool, "Pool object is null.");

			return pool.Get();
		}
		public void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable
		{
			if (!_monoPools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;

				Assert.IsNotNull(pool, "Pool object is null.");

				pool.Release(obj);
			}
		}
		public void ReleasePure<T>(T obj) where T : class, IPoolable
		{
			if (!_purePools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				PurePool<T> pool = poolObj as PurePool<T>;

				Assert.IsNotNull(pool, "Pool object is null.");

				pool.Release(obj);
			}
		}
		public void DestroyMono<T>(T obj) where T : MonoBehaviour, IPoolable
		{
			if (!_monoPools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;
				
				Assert.IsNotNull(pool, "Pool object is null.");
				
				pool.Destroy(obj);
			}
		}
		public void DestroyPure<T>(T obj) where T : class, IPoolable
		{
			if (!_purePools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				PurePool<T> pool = poolObj as PurePool<T>;
				
				Assert.IsNotNull(pool, "Pool object is null.");
				
				pool.Destroy(obj);
			}
		}
		public void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable
		{
			if (!_monoPools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;

				Assert.IsNotNull(pool, "Pool object is null.");

				pool.ReleaseAll();
			}
		}
		public void ReleaseAllPure<T>() where T : class, IPoolable
		{
			if (!_purePools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				PurePool<T> pool = poolObj as PurePool<T>;

				Assert.IsNotNull(pool, "Pool object is null.");

				pool.ReleaseAll();
			}
		}
		public void DestroyAllMono<T>() where T : MonoBehaviour, IPoolable
		{
			if (!_monoPools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				MonoPool<T> pool = poolObj as MonoPool<T>;

				Assert.IsNotNull(pool, "Pool object is null.");

				pool.DestroyAll();
			}
		}
		public void DestroyAllPure<T>() where T : class, IPoolable
		{
			if (!_purePools.TryGetValue(typeof(T), out object poolObj))
				PoolServiceUtilities.LogNoPoolFoundError<T>();
			else
			{
				PurePool<T> pool = poolObj as PurePool<T>;

				Assert.IsNotNull(pool, "Pool object is null.");

				pool.DestroyAll();
			}
		}
        #endregion
    }
    
    public class TestOne : PurePoolable
    {
	    public override void OnCreated()
	    {
		    throw new NotImplementedException();
	    }

	    public override void OnGetFromPool()
	    {
		    throw new NotImplementedException();
	    }

	    public override void OnReturnToPool()
	    {
		    throw new NotImplementedException();
	    }

	    public override void OnDestroyed()
	    {
		    throw new NotImplementedException();
	    }
    }
    
    public class TestTwo : MonoPoolable
    {
	    public override void OnCreated()
	    {
		    throw new NotImplementedException();
	    }

	    public override void OnGetFromPool()
	    {
		    throw new NotImplementedException();
	    }

	    public override void OnReturnToPool()
	    {
		    throw new NotImplementedException();
	    }

	    public override void OnDestroyed()
	    {
		    throw new NotImplementedException();
	    }
    }
}