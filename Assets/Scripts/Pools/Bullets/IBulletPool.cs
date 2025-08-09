using Factories;
using PlayerRelated;
using UnityEngine;

namespace Pools
{
    public interface IBulletPool
    {
        public Bullet Get(BulletType type, BulletConfigs config, Vector3 position, Quaternion rotation);
        public void Release(Bullet bullet);
    }
}