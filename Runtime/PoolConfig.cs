using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "HMPool/PoolConfig")]
    public sealed class PoolConfig : ScriptableObject
    {
        #region Fields
        [Header("Pool Config Fields")]
        [SerializeField] private PoolDatum[] poolData;
        #endregion

        #region Properities
        public PoolDatum[] PoolData
        {
            get => poolData;
            set => poolData = value;
        }
        #endregion
    }
}