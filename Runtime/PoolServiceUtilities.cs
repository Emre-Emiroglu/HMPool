using System;
using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    public static class PoolServiceUtilities
    {
        #region Fields
        private static PoolService _poolService;
        private static bool _isInitialized;
        #endregion

        #region Core
        public static void Initialize(PoolConfig poolConfig)
        {
            _poolService = new PoolService(poolConfig);
            
            _poolService.Initialize();
        }
        #endregion

        #region Executes
        public static T GetMono<T>() where T : MonoBehaviour, IPoolable => _poolService.GetMono<T>();
		public static T GetPure<T>() where T : class, IPoolable => _poolService.GetPure<T>();
		public static void ReleaseMono<T>(T obj) where T : MonoBehaviour, IPoolable => _poolService.ReleaseMono(obj);
		public static void ReleasePure<T>(T obj) where T : class, IPoolable => _poolService.ReleasePure(obj);
		public static void DestroyMono<T>(T obj) where T : MonoBehaviour, IPoolable => _poolService.DestroyMono(obj);
		public static void DestroyPure<T>(T obj) where T : class, IPoolable => _poolService.DestroyPure(obj);
		public static void ReleaseAllMono<T>() where T : MonoBehaviour, IPoolable => _poolService.ReleaseAllMono<T>();
		public static void ReleaseAllPure<T>() where T : class, IPoolable => _poolService.ReleaseAllPure<T>();
		public static void DestroyAllMono<T>() where T : MonoBehaviour, IPoolable => _poolService.DestroyAllMono<T>();
		public static void DestroyAllPure<T>() where T : class, IPoolable => _poolService.DestroyAllPure<T>();
		public static void ThrowNoPoolFoundException<T>() where T : class, IPoolable =>
			throw new InvalidOperationException($"No pool found for type {typeof(T)}.");
		public static void LogNoPoolFoundError<T>() where T : class, IPoolable =>
			Debug.LogError($"No pool found for type {typeof(T)}.");
		#endregion
    }
}