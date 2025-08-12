using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Particles
{
    public class ParticlesPool: MonoBehaviour, IParticlesPool
    {
        [Serializable]
        private class ParticlePoolEntry
        {
            public DieParticlesType Type;
            public DestrParticleUnit Prefab;
            public int InitialSize = 10;
        }
        public event Action<bool> OnForceStop;
      
        [SerializeField] private List<ParticlePoolEntry> _bulletPools = new();
        [SerializeField] private Transform _vfxRoot;
        
        private readonly Dictionary<DieParticlesType, Queue<IDestrParticle>> _pools = new();
        private readonly Dictionary<DieParticlesType, ParticlePoolEntry> _entries = new();

        public void Initialize()
        {
            foreach (var entry in _bulletPools)
            {
                _entries.Add(entry.Type, entry);
            }
            
            foreach (var entry in _entries)
            {
                var queue = new Queue<IDestrParticle>();
                for (int i = 0; i < entry.Value.InitialSize; i++)
                {
                    var vfx = Object.Instantiate(entry.Value.Prefab, _vfxRoot).GetComponent<IDestrParticle>();
                    vfx.SetPool(this);
                    vfx.OnDespawned();
                    queue.Enqueue(vfx);

                }

                _pools[entry.Key] = queue;
            }
        }

        public IDestrParticle Get(DieParticlesType type, Vector3 position, Quaternion rotation)
        {
            
            if (!_pools.TryGetValue(type, out var queue))
            {
                Debug.LogError($"No pool found for bullet type {type}");
                return null;
            }


            var vfx = queue.Count > 0 ? queue.Dequeue() : CreateNewBullet(type);
            vfx.Transform.position = position;
            vfx.Transform.rotation = rotation;
          
            vfx.OnSpawned();

           // bullet.gameObject.SetActive(true);
            return vfx;
        }

        public void Release(IDestrParticle vfx)
        {
            vfx.OnDespawned();
           
            var type = vfx.DieParticlesType; // Add this property to Bullet class
            if (_pools.TryGetValue(type, out var queue))
            {
                queue.Enqueue(vfx);
            }
            else
            {
                Debug.LogWarning($"Pool missing for bullet type {type} on release. Destroying bullet.");
               Destroy(vfx.Transform.gameObject);
            }
        }

        private IDestrParticle CreateNewBullet(DieParticlesType type)
        {
            var entry = _entries[type];
            if (entry == null)
            {
                Debug.LogError($"No bullet prefab found for bullet type {type}");
                return null;
            }

            var vfx = Instantiate(entry.Prefab, _vfxRoot).GetComponent<IDestrParticle>();
            vfx.SetPool(this);
            _pools[type].Enqueue(vfx);
            vfx.OnDespawned();
            return vfx;
        }
        
        
        private void ForceStop(bool on)
        {
            OnForceStop?.Invoke(on);
        }

    }
}