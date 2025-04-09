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

        #region Core
        public void Initialize()
        {
	        CreateDontDestroyOnLoadParent();
	        InitializeAllPools();
        }
        #endregion

        #region Executes
        public T GetMono<T>() where T : MonoBehaviour, IPoolable => GetPool<MonoPool<T>, T>(_monoPools).Get();
        public T GetPure<T>() where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).Get();
        public void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable => GetPool<MonoPool<T>, T>(_monoPools).Release(obj);
        public void ReleasePure<T>(T obj) where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).Release(obj);
        public void DestroyMono<T>(T obj) where T : MonoBehaviour, IPoolable => GetPool<MonoPool<T>, T>(_monoPools).Destroy(obj);
        public void DestroyPure<T>(T obj) where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).Destroy(obj);
        public void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable => GetPool<MonoPool<T>, T>(_monoPools).ReleaseAll();
        public void ReleaseAllPure<T>() where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).ReleaseAll();
        public void DestroyAllMono<T>() where T : MonoBehaviour, IPoolable => GetPool<MonoPool<T>, T>(_monoPools).DestroyAll();
        public void DestroyAllPure<T>() where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).DestroyAll();
        private void CreateDontDestroyOnLoadParent()
        {
            _poolParent = new GameObject("PoolParent").transform;
            
            Object.DontDestroyOnLoad(_poolParent);
        }
        private void InitializeAllPools()
        {
	        foreach (var datum in _config.PoolData)
	        {
		        Type poolType = datum.IsMono ? typeof(MonoPool<>) : typeof(PurePool<>);
		        Dictionary<Type, object> poolDict = datum.IsMono ? _monoPools : _purePools;
		        object poolInstance = CreatePoolInstance(poolType, datum, datum.IsMono ? _poolParent : null);

		        poolDict[datum.ClassType] = poolInstance;
	        }
        }
        private object CreatePoolInstance(Type baseType, PoolDatum datum, Transform parent = null)
        {
	        Type genericType = baseType.MakeGenericType(datum.ClassType);
	        object[] parameters = parent != null ? new object[] { datum, parent } : new object[] { datum };
	        
	        return Activator.CreateInstance(genericType, parameters);
        }
        private TPool GetPool<TPool, T>(Dictionary<Type, object> poolDict)
	        where TPool : PoolBase<T> where T : class, IPoolable
        {
	        if (!poolDict.TryGetValue(typeof(T), out object poolObj))
		        PoolServiceUtilities.ThrowNoPoolFoundException<T>();

	        TPool pool = poolObj as TPool;

	        Assert.IsNotNull(pool, "Pool object is null.");

	        return pool;
        }
        #endregion
    }
}