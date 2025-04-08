using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    public abstract class MonoPoolable : MonoBehaviour, IPoolable
    {
        #region Core
        public abstract void OnCreated();
        public abstract void OnGetFromPool();
        public abstract void OnReturnToPool();
        public abstract void OnDestroyed();
        #endregion
    }
}