namespace CodeCatGames.HMPool.Runtime
{
    public interface IPoolable
    {
        public void OnCreated();
        public void OnGetFromPool();
        public void OnReturnToPool();
        public void OnDestroyed();
    }
}