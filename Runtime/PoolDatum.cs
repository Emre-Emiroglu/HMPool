using System;
using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    /// <summary>
    /// PoolDatum defines the configuration for a single object pool.
    /// It contains settings such as the pool's size, the prefab for MonoBehaviour-based objects, 
    /// and the class type of the objects in the pool.
    /// </summary>
    [Serializable]
    public struct PoolDatum
    {
        #region Fields
        [Header("Pool Datum Fields")]
        [SerializeField] private bool isMono;
        [SerializeField] private GameObject monoPrefab;
        [SerializeField] private string classTypeName;
        [SerializeField] private int initialSize;
        [SerializeField] private int defaultCapacity;
        [SerializeField] private int maximumSize;
        #endregion

        #region Properities
        /// <summary>
        /// Gets or sets a value indicating whether this pool is for MonoBehaviour-based objects.
        /// </summary>
        public bool IsMono
        {
            get => isMono;
            set => isMono = value;
        }
        
        /// <summary>
        /// Gets or sets the prefab for MonoBehaviour-based objects in this pool.
        /// </summary>
        public GameObject MonoPrefab
        {
            get => monoPrefab;
            set => monoPrefab = value;
        }
        
        /// <summary>
        /// Gets or sets the initial size of the pool.
        /// </summary>
        public int InitialSize
        {
            get => initialSize;
            set => initialSize = value;
        }
        
        /// <summary>
        /// Gets or sets the default capacity of the pool.
        /// </summary>
        public int DefaultCapacity
        {
            get => defaultCapacity;
            set => defaultCapacity = value;
        }
        
        /// <summary>
        /// Gets or sets the maximum size of the pool.
        /// </summary>
        public int MaximumSize
        {
            get => maximumSize;
            set => maximumSize = value;
        }
        
        /// <summary>
        /// Gets or sets the class type for the objects in this pool.
        /// </summary>
        public Type ClassType
        {
            get => Type.GetType(classTypeName);
            set => classTypeName = value?.AssemblyQualifiedName;
        }
        #endregion
    }
}