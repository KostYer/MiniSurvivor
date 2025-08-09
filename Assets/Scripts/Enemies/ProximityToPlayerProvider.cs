using PlayerRelated;
using UnityEngine;

namespace Enemies
{
    public class ProximityToPlayerProvider: MonoBehaviour, ITargetProximityProvider
    {
        private Transform _target;
        public void Initialize(Transform target)
        {
            _target = target;
        }

        public bool IsTargetClose(out Vector3 targetPos, float range)
        {
            targetPos = _target.position;
            return Vector3.Distance(transform.position, targetPos) <= range;
        }
    }
}