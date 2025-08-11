using Particles;
using UnityEngine;

namespace PlayerRelated
{
    public class DestructionDataHolder: MonoBehaviour
    {
        [SerializeField] private DieParticlesType _dieParticlesType;
        [SerializeField] private float _yoffset = .7f;
        public DieParticlesType DieParticlesType => _dieParticlesType;
        public float YSpawnOffset => _yoffset;
    }
}