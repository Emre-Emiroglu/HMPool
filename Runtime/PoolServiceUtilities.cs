using System;
using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
	/// <summary>
	/// The PoolServiceUtilities class provides static methods for interacting with the PoolService.
	/// It allows easy access to MonoBehaviour and non-MonoBehaviour pools, including getting, releasing, and destroying objects.
	/// </summary>
    public static class PoolServiceUtilities
    {
        #region Fields
        private static PoolService _poolService;
        #endregion

        #region Core
        /// <summary>
        /// Initializes the PoolService with the provided pool configuration and prepares the pools for use.
        /// </summary>
        /// <param name="poolConfig">The configuration for the pools.</param>
        public static void Initialize(PoolConfig poolConfig)
        {
            _poolService = new PoolService(poolConfig);
            
            _poolService.Initialize();
        }
        #endregion

        #region Executes
        /// <summary>
        /// Retrieves an object from the MonoBehaviour-based pool for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the poolable MonoBehaviour object.</typeparam>
        /// <returns>The poolable object from the pool.</returns>
        public static T GetMono<T>() where T : MonoBehaviour, IPoolable => _poolService.GetMono<T>();
        
        /// <summary>
        /// Retrieves an object from the non-MonoBehaviour-based pool for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the poolable object.</typeparam>
        /// <returns>The poolable object from the pool.</returns>
		public static T GetPure<T>() where T : class, IPoolable => _poolService.GetPure<T>();
		
		/// <summary>
		/// Releases the specified MonoBehaviour object back to its pool.
		/// </summary>
		/// <typeparam name="T">The type of the MonoBehaviour object to release.</typeparam>
		/// <param name="obj">The object to release.</param>
		public static void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable => _poolService.ReleaseMono(obj);
		
		/// <summary>
		/// Releases the specified non-MonoBehaviour object back to its pool.
		/// </summary>
		/// <typeparam name="T">The type of the non-MonoBehaviour object to release.</typeparam>
		/// <param name="obj">The object to release.</param>
		public static void ReleasePure<T>(T obj) where T : class, IPoolable => _poolService.ReleasePure(obj);
		
		/// <summary>
		/// Destroys the specified MonoBehaviour object and removes it from the pool.
		/// </summary>
		/// <typeparam name="T">The type of the MonoBehaviour object to destroy.</typeparam>
		/// <param name="obj">The object to destroy.</param>
		public static void DestroyMono<T>(T obj) where T : MonoBehaviour, IPoolable => _poolService.DestroyMono(obj);
		
		/// <summary>
		/// Destroys the specified non-MonoBehaviour object and removes it from the pool.
		/// </summary>
		/// <typeparam name="T">The type of the non-MonoBehaviour object to destroy.</typeparam>
		/// <param name="obj">The object to destroy.</param>
		public static void DestroyPure<T>(T obj) where T : class, IPoolable => _poolService.DestroyPure(obj);
		
		/// <summary>
		/// Releases all objects in the MonoBehaviour-based pool.
		/// </summary>
		/// <typeparam name="T">The type of MonoBehaviour objects to release.</typeparam>
		public static void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable => _poolService.ReleaseAllMono<T>();
		
		/// <summary>
		/// Releases all objects in the non-MonoBehaviour-based pool.
		/// </summary>
		/// <typeparam name="T">The type of non-MonoBehaviour objects to release.</typeparam>
		public static void ReleaseAllPure<T>() where T : class, IPoolable => _poolService.ReleaseAllPure<T>();
		
		/// <summary>
		/// Destroys all objects in the MonoBehaviour-based pool.
		/// </summary>
		/// <typeparam name="T">The type of MonoBehaviour objects to destroy.</typeparam>
		public static void DestroyAllMono<T>() where T : MonoBehaviour, IPoolable => _poolService.DestroyAllMono<T>();
		
		/// <summary>
		/// Destroys all objects in the non-MonoBehaviour-based pool.
		/// </summary>
		/// <typeparam name="T">The type of non-MonoBehaviour objects to destroy.</typeparam>
		public static void DestroyAllPure<T>() where T : class, IPoolable => _poolService.DestroyAllPure<T>();
		
		/// <summary>
		/// Throws an exception indicating no pool was found for the specified type.
		/// </summary>
		/// <typeparam name="T">The type of the poolable object.</typeparam>
		public static void ThrowNoPoolFoundException<T>() where T : IPoolable =>
			throw new InvalidOperationException($"No pool found for type {typeof(T)}.");
		
		/// <summary>
		/// Logs an error indicating no pool was found for the specified type.
		/// </summary>
		/// <typeparam name="T">The type of the poolable object.</typeparam>
		public static void LogNoPoolFoundError<T>() where T : IPoolable =>
			Debug.LogError($"No pool found for type {typeof(T)}.");
		#endregion
    }
}