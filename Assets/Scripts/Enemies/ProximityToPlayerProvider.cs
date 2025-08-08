using PlayerRelated;
using UnityEngine;

namespace Enemies
{
    public class ProximityToPlayerProvider: MonoBehaviour, ITargetProximityProvider
    {
        public bool IsTargetClose(out Vector3 targetPos, float range)
        {
            targetPos = default;
            return false;
        }
    }
}