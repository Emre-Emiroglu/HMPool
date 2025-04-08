using System.Collections.Generic;
using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "HMPool/PoolConfig")]
    public sealed class PoolConfig : ScriptableObject
    {
        #region Fields
        [Header("Pool Config Fields")]
        [SerializeField] private List<PoolDatum> poolData;
        #endregion

        #region Getters
        public List<PoolDatum> PoolData => poolData;
        #endregion
    }
}