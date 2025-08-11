using Particles;
using PlayerRelated;

namespace Pools
{
    public interface IBulletPoolable
    {
        BulletType BulletType { get;  }
        void OnSpawned();
        void OnDespawned();
        void SetPool(IBulletPool pool);
    }
}