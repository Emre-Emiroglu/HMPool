using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    public sealed class MonoPool<T> : PoolBase<T> where T : MonoBehaviour, IPoolable
    {
        #region ReadonlyFields
        private readonly Transform _parent;
        #endregion
        
        #region Constructor
        public MonoPool(PoolDatum poolDatum, Transform parent) : base(poolDatum)
        {
            _parent = parent;

            InstantiateDefaultObjects();
        }
        #endregion

        #region Core
        protected override T CreateObject()
        {
            T obj = Object.Instantiate(PoolDatum.MonoPrefab, _parent).GetComponent<T>();
            
            obj.OnCreated();
			
            return obj;
        }
        protected override void GetFromPool(T obj)
        {
            obj.OnGetFromPool();
            
            obj.gameObject.SetActive(true);
        }
        protected override void ReturnToPool(T obj)
        {
            obj.OnReturnToPool();
            
            obj.gameObject.SetActive(false);
        }
        protected override void DestroyObject(T obj)
        {
            obj.OnDestroyed();
            
            Object.Destroy(obj.gameObject);
        }
        #endregion
    }
}