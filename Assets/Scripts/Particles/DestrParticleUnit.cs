using UnityEngine;

namespace Particles
{
    public class DestrParticleUnit: MonoBehaviour, IDestrParticle
    {
        public DieParticlesType DieParticlesType { get; }
        public Transform Transform { get; private set; }

        private IParticlesPool _pool;

        [SerializeField] private ParticleSystem _particleSystem;

        private void OnValidate()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
         
        }

        public void OnSpawned()
        {
            Transform = transform;
            gameObject.SetActive(true);
            _particleSystem.Play();
        }

        public void OnDespawned()
        {
            Transform = transform;
            gameObject.SetActive(false);
        }

        public void SetPool(IParticlesPool pool)
        {
            _pool = pool;
        }
    }
}