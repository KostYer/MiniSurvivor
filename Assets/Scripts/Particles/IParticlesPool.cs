using Enemies;
using UnityEngine;

namespace Particles
{
    
    public interface IParticlesPool
    {
        public IDestrParticle Get(DieParticlesType type, Vector3 position, Quaternion rotation);
        public void Release(IDestrParticle vfx);
    }
}