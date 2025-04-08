using System;
using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
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

        #region Getters
        public bool IsMono => isMono;
        public GameObject MonoPrefab => monoPrefab;
        public int InitialSize => initialSize;
        public int DefaultCapacity => defaultCapacity;
        public int MaximumSize => maximumSize;
        #endregion

        #region Properities
        public Type ClassType
        {
            get => Type.GetType(classTypeName);
            set => classTypeName = value?.AssemblyQualifiedName;
        }
        #endregion
    }
}