using Enemies;
using UnityEngine;

namespace Particles
{
    public interface IDestrParticle
    {
        DieParticlesType DieParticlesType { get;  }
        public Transform Transform { get; }
        void OnSpawned();
        void OnDespawned();
        void SetPool(IParticlesPool pool);
    }
}