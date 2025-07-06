using UnityEngine;

namespace HMPool.Runtime
{
    /// <summary>
    /// PoolConfig is a ScriptableObject that holds configuration data for object pools.
    /// It contains an array of PoolDatum, each of which defines the settings for a specific pool.
    /// </summary>
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "HMPool/PoolConfig")]
    public sealed class PoolConfig : ScriptableObject
    {
        #region Fields
        [Header("Pool Config Fields")]
        [SerializeField] private PoolDatum[] poolData;
        #endregion

        #region Properities
        /// <summary>
        /// Gets or sets the pool data for this configuration.
        /// </summary>
        /// <value>The array of PoolDatum containing the pool configurations.</value>
        public PoolDatum[] PoolData
        {
            get => poolData;
            set => poolData = value;
        }
        #endregion
    }
}