using Core;
using UnityEngine;

namespace PlayerRelated
{
    public interface ITargetProvider
    {
        public bool TryFindTarget(out Vector3 targetPos, float range);
    }

    public class PlayerTargetProvider: MonoBehaviour, ITargetProvider
    {

        public IWavesProvider _wavesProvider;
        
        public void Initialize(IWavesProvider wavesProvider)
        {
            _wavesProvider = wavesProvider;
        }

        public bool TryFindTarget(out Vector3 target, float range)
        {
            target = default;
            return false;
        }

      
    }
}