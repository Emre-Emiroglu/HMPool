using System.Collections;
using CodeCatGames.HMPool.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CodeCatGames.HMPool.Tests.PlayMode
{
    public sealed class PoolServicePlayModeTests
    {
        private PoolConfig _poolConfig;
        
        [SetUp]
        public void SetUp()
        {
            _poolConfig = ScriptableObject.CreateInstance<PoolConfig>();
            
            GameObject monoPoolableObject = new GameObject();
            monoPoolableObject.AddComponent<MyMonoPoolable>();
            
            PoolDatum[] poolData = new PoolDatum[2];
            
            poolData[0] = new PoolDatum
            {
                IsMono = true,
                MonoPrefab = monoPoolableObject,
                ClassType = typeof(MyMonoPoolable),
                InitialSize = 2,
                DefaultCapacity = 5,
                MaximumSize = 10
            };
            
            poolData[1] = new PoolDatum
            {
                IsMono = false,
                ClassType = typeof(MyPurePoolable),
                InitialSize = 2,
                DefaultCapacity = 5,
                MaximumSize = 10
            };
            
            _poolConfig.PoolData = poolData;

            PoolServiceUtilities.Initialize(_poolConfig);
        }

        [TearDown]
        public void TearDown() => _poolConfig = null;

        [UnityTest]
        public IEnumerator TestMonoPool_Create_Get_Release_Destroy()
        {
            MyMonoPoolable poolable = PoolServiceUtilities.GetMono<MyMonoPoolable>();
            
            yield return null;
            
            Assert.IsTrue(poolable.IsCreated);
            
            Assert.IsTrue(poolable.IsGetted);
            
            PoolServiceUtilities.ReleaseMono(poolable);
            
            yield return null;
            
            Assert.IsTrue(poolable.IsReturnedToPool);

            PoolServiceUtilities.DestroyMono(poolable);
            
            yield return null;
            
            Assert.IsTrue(poolable.IsDestroyed);
        }

        [UnityTest]
        public IEnumerator TestPurePool_Create_Get_Release_Destroy()
        {
            MyPurePoolable poolable = PoolServiceUtilities.GetPure<MyPurePoolable>();
            
            yield return null;
            
            Assert.IsTrue(poolable.IsCreated);
            
            Assert.IsTrue(poolable.IsGetted);
            
            PoolServiceUtilities.ReleasePure(poolable);
            
            yield return null;
            
            Assert.IsTrue(poolable.IsReturnedToPool);

            PoolServiceUtilities.DestroyPure(poolable);
            
            yield return null;
            
            Assert.IsTrue(poolable.IsDestroyed);
        }

        [UnityTest]
        public IEnumerator TestMonoPool_ReleaseAll_DestroyAll()
        {
            MyMonoPoolable poolable1 = PoolServiceUtilities.GetMono<MyMonoPoolable>();
            MyMonoPoolable poolable2 = PoolServiceUtilities.GetMono<MyMonoPoolable>();

            PoolServiceUtilities.ReleaseAllMono<MyMonoPoolable>();
            PoolServiceUtilities.DestroyAllMono<MyMonoPoolable>();
            
            yield return null;
            
            Assert.IsTrue(poolable1.IsDestroyed);
            Assert.IsTrue(poolable2.IsDestroyed);
        }

        [UnityTest]
        public IEnumerator TestPurePool_ReleaseAll_DestroyAll()
        {
            MyPurePoolable poolable1 = PoolServiceUtilities.GetPure<MyPurePoolable>();
            MyPurePoolable poolable2 = PoolServiceUtilities.GetPure<MyPurePoolable>();

            PoolServiceUtilities.ReleaseAllPure<MyPurePoolable>();
            PoolServiceUtilities.DestroyAllPure<MyPurePoolable>();
            
            yield return null;

            Assert.IsTrue(poolable1.IsDestroyed);
            Assert.IsTrue(poolable2.IsDestroyed);
        }
        
        private class MyMonoPoolable : MonoPoolable
        {
            public bool IsCreated { get; private set; }
            public bool IsGetted { get; private set; }
            public bool IsReturnedToPool { get; private set; }
            public bool IsDestroyed { get; private set; }

            public override void OnCreated() => IsCreated = true;
            public override void OnGetFromPool() => IsGetted = true;
            public override void OnReturnToPool() => IsReturnedToPool = true;
            public override void OnDestroyed() => IsDestroyed = true;
        }

        private class MyPurePoolable : PurePoolable
        {
            public bool IsCreated { get; private set; }
            public bool IsGetted { get; private set; }
            public bool IsReturnedToPool { get; private set; }
            public bool IsDestroyed { get; private set; }

            public override void OnCreated() => IsCreated = true;
            public override void OnGetFromPool() => IsGetted = true;
            public override void OnReturnToPool() => IsReturnedToPool = true;
            public override void OnDestroyed() => IsDestroyed = true;
        }
    }
}