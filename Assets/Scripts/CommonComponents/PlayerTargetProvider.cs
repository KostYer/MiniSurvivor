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
            if (_wavesProvider.Enemies == null) return false;
            if (_wavesProvider.Enemies.Count == 0) return false;

            var minDistance = float.MaxValue;

            for (int i = 0; i < _wavesProvider.Enemies.Count; i++)
            {
                var enemy = _wavesProvider.Enemies[i];
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
             //   if (distance > range) continue;
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = enemy.transform.position;
                }
            }
 
            return minDistance <= range;
        }

      
    }
}