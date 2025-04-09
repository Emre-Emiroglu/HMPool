using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace CodeCatGames.HMPool.Runtime
{
	/// <summary>
	/// The PoolService class manages and operates the object pools for both MonoBehaviour and non-MonoBehaviour based poolable objects.
	/// It provides methods to get, release, and destroy poolable objects.
	/// </summary>
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
        /// <summary>
        /// Initializes a new instance of the PoolService class with the provided configuration.
        /// </summary>
        /// <param name="config">The pool configuration.</param>
        public PoolService(PoolConfig config) => _config = config;
        #endregion

        #region Core
        /// <summary>
        /// Initializes the PoolService by creating the parent for MonoBehaviour pools and initializing all pools based on the provided configuration.
        /// </summary>
        public void Initialize()
        {
	        CreateDontDestroyOnLoadParent();
	        InitializeAllPools();
        }
        #endregion

        #region Executes
        /// <summary>
        /// Retrieves an object from the MonoBehaviour-based pool for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the poolable MonoBehaviour object.</typeparam>
        /// <returns>The poolable object from the pool.</returns>
        public T GetMono<T>() where T : MonoBehaviour, IPoolable => GetPool<MonoPool<T>, T>(_monoPools).Get();
        
        /// <summary>
        /// Retrieves an object from the non-MonoBehaviour-based pool for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the poolable object.</typeparam>
        /// <returns>The poolable object from the pool.</returns>
        public T GetPure<T>() where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).Get();

        /// <summary>
        /// Releases the specified MonoBehaviour object back to its pool.
        /// </summary>
        /// <typeparam name="T">The type of the MonoBehaviour object to release.</typeparam>
        /// <param name="obj">The object to release.</param>
        public void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable =>
	        GetPool<MonoPool<T>, T>(_monoPools).Release(obj);

        /// <summary>
        /// Releases the specified non-MonoBehaviour object back to its pool.
        /// </summary>
        /// <typeparam name="T">The type of the non-MonoBehaviour object to release.</typeparam>
        /// <param name="obj">The object to release.</param>
        public void ReleasePure<T>(T obj) where T : class, IPoolable =>
	        GetPool<PurePool<T>, T>(_purePools).Release(obj);

        /// <summary>
        /// Destroys the specified MonoBehaviour object and removes it from the pool.
        /// </summary>
        /// <typeparam name="T">The type of the MonoBehaviour object to destroy.</typeparam>
        /// <param name="obj">The object to destroy.</param>
        public void DestroyMono<T>(T obj) where T : MonoBehaviour, IPoolable =>
	        GetPool<MonoPool<T>, T>(_monoPools).Destroy(obj);

        /// <summary>
        /// Destroys the specified non-MonoBehaviour object and removes it from the pool.
        /// </summary>
        /// <typeparam name="T">The type of the non-MonoBehaviour object to destroy.</typeparam>
        /// <param name="obj">The object to destroy.</param>
        public void DestroyPure<T>(T obj) where T : class, IPoolable =>
	        GetPool<PurePool<T>, T>(_purePools).Destroy(obj);

        /// <summary>
        /// Releases all objects in the MonoBehaviour-based pool.
        /// </summary>
        /// <typeparam name="T">The type of MonoBehaviour objects to release.</typeparam>
        public void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable =>
	        GetPool<MonoPool<T>, T>(_monoPools).ReleaseAll();
        
        /// <summary>
        /// Releases all objects in the non-MonoBehaviour-based pool.
        /// </summary>
        /// <typeparam name="T">The type of non-MonoBehaviour objects to release.</typeparam>
        public void ReleaseAllPure<T>() where T : class, IPoolable => GetPool<PurePool<T>, T>(_purePools).ReleaseAll();

        /// <summary>
        /// Destroys all objects in the MonoBehaviour-based pool.
        /// </summary>
        /// <typeparam name="T">The type of MonoBehaviour objects to destroy.</typeparam>
        public void DestroyAllMono<T>() where T : MonoBehaviour, IPoolable =>
	        GetPool<MonoPool<T>, T>(_monoPools).DestroyAll();

        /// <summary>
        /// Destroys all objects in the non-MonoBehaviour-based pool.
        /// </summary>
        /// <typeparam name="T">The type of non-MonoBehaviour objects to destroy.</typeparam>
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