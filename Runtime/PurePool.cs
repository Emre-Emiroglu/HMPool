using System;

namespace CodeCatGames.HMPool.Runtime
{
    public sealed class PurePool<T> : PoolBase<T> where T : class, IPoolable
    {
        #region Constructor
        public PurePool(PoolDatum poolDatum) : base(poolDatum) => InstantiateDefaultObjects();
        #endregion

        #region Core
        protected override T CreateObject()
        {
            T obj = Activator.CreateInstance<T>();
            
            obj.OnCreated();
			
            return obj;
        }
        protected override void GetFromPool(T obj) => obj.OnGetFromPool();
        protected override void ReturnToPool(T obj) => obj.OnReturnToPool();
        protected override void DestroyObject(T obj) => obj.OnDestroyed();
        #endregion
    }
}