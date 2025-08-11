using System;
using Factories;
using PlayerRelated;
using UnityEngine;

namespace Pools
{
    public interface IBulletPool
    {
        public event Action<bool> OnForceStop;
        public Bullet Get(BulletType type, BulletConfigs config, Vector3 position, Quaternion rotation);
        public void Release(Bullet bullet);
    }
}