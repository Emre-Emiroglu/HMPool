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

        #region Properities
        public bool IsMono
        {
            get => isMono;
            set => isMono = value;
        }
        public GameObject MonoPrefab
        {
            get => monoPrefab;
            set => monoPrefab = value;
        }
        public int InitialSize
        {
            get => initialSize;
            set => initialSize = value;
        }
        public int DefaultCapacity
        {
            get => defaultCapacity;
            set => defaultCapacity = value;
        }
        public int MaximumSize
        {
            get => maximumSize;
            set => maximumSize = value;
        }
        public Type ClassType
        {
            get => Type.GetType(classTypeName);
            set => classTypeName = value?.AssemblyQualifiedName;
        }
        #endregion
    }
}