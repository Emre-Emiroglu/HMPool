namespace HMPool.Runtime
{
    /// <summary>
    /// Interface to be implemented by poolable objects that can be managed in a pool.
    /// Provides methods for different lifecycle stages of an object within the pool.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Called when the object is created and added to the pool.
        /// </summary>
        public void OnCreated();
        
        /// <summary>
        /// Called when the object is retrieved from the pool.
        /// </summary>
        public void OnGetFromPool();
        
        /// <summary>
        /// Called when the object is returned to the pool.
        /// </summary>
        public void OnReturnToPool();
        
        /// <summary>
        /// Called when the object is destroyed and removed from the pool.
        /// </summary>
        public void OnDestroyed();
    }
}