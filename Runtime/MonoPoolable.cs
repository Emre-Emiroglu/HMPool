using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    /// <summary>
    /// Abstract class to be inherited by MonoBehaviour-based poolable objects.
    /// Implements the IPoolable interface, providing the core lifecycle methods for poolable objects.
    /// </summary>
    public abstract class MonoPoolable : MonoBehaviour, IPoolable
    {
        #region Core
        /// <summary>
        /// Called when the object is created and added to the pool.
        /// This method must be implemented by the derived class to specify custom behavior on creation.
        /// </summary>
        public abstract void OnCreated();
        
        /// <summary>
        /// Called when the object is retrieved from the pool.
        /// This method must be implemented by the derived class to specify custom behavior when the object is used.
        /// </summary>
        public abstract void OnGetFromPool();
        
        /// <summary>
        /// Called when the object is returned to the pool.
        /// This method must be implemented by the derived class to specify custom behavior when the object is no longer in use.
        /// </summary>
        public abstract void OnReturnToPool();
        
        /// <summary>
        /// Called when the object is destroyed and removed from the pool.
        /// This method must be implemented by the derived class to specify custom behavior during object destruction.
        /// </summary>
        public abstract void OnDestroyed();
        #endregion
    }
}