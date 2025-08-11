using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "BulletConfigsSO", menuName = "Factories/BulletConfigs")]
    public class BulletConfigs: ScriptableObject
    {
    //    public GameObject Prefab;
        public float Speed;
        public float Damage;
        public float Lifetime;
        public LayerMask CollisionMask;
        public BulletTeam Team; // Player, Enemy, Neutral
    }
}