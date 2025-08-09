using System.Collections.Generic;
using Factories;
using PlayerRelated;
using UnityEngine;

namespace Pools
{
    public class BulletPool : MonoBehaviour, IBulletPool
    {

        [System.Serializable]
        private class BulletPoolEntry
        {
            public BulletType Type;
            public Bullet Prefab;
            public int InitialSize = 10;
        }
      
        [SerializeField] private List<BulletPoolEntry> _bulletPools = new();
        [SerializeField] private Transform _bulletsRoot;
        
        private readonly Dictionary<BulletType, Queue<Bullet>> _pools = new();
        private readonly Dictionary<BulletType, BulletPoolEntry> _entries = new();

        public void Initialize()
        {
            foreach (var entry in _bulletPools)
            {
                _entries.Add(entry.Type, entry);
            }
            
            foreach (var entry in _entries)
            {
                var queue = new Queue<Bullet>();
                for (int i = 0; i < entry.Value.InitialSize; i++)
                {
                    var bullet = Object.Instantiate(entry.Value.Prefab, _bulletsRoot).GetComponent<Bullet>();
                    bullet.SetPool(this);
                    bullet.OnDespawned();
                    queue.Enqueue(bullet);
                }

                _pools[entry.Key] = queue;
            }
        }

        public Bullet Get(BulletType type, BulletConfigs config, Vector3 position, Quaternion rotation)
        {
            
            if (!_pools.TryGetValue(type, out var queue))
            {
                Debug.LogError($"No pool found for bullet type {type}");
                return null;
            }


            Bullet bullet = queue.Count > 0 ? queue.Dequeue() : CreateNewBullet(type);
            bullet.SetConfig(config);
            bullet.transform.position = position;
          
            bullet.OnSpawned();

            bullet.gameObject.SetActive(true);
            return bullet;
        }

        public void Release(Bullet bullet)
        {
            bullet.OnDespawned();
           
            var type = bullet.BulletType; // Add this property to Bullet class
            if (_pools.TryGetValue(type, out var queue))
            {
                queue.Enqueue(bullet);
            }
            else
            {
                Debug.LogWarning($"Pool missing for bullet type {type} on release. Destroying bullet.");
                Destroy(bullet.gameObject);
            }
        }

        private Bullet CreateNewBullet(BulletType type)
        {
            var entry = _entries[type];
            if (entry == null)
            {
                Debug.LogError($"No bullet prefab found for bullet type {type}");
                return null;
            }

            var bullet = Instantiate(entry.Prefab, _bulletsRoot);
            bullet.SetPool(this);
            _pools[type].Enqueue(bullet);
            bullet.OnDespawned();
            return bullet;
        }
        
    }
}