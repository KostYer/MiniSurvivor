using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ShootSettingsSO", menuName = "Settings/ShootSettings")]
    public class ShootSettings : ScriptableObject
    {
        [SerializeField] private float _shootRange;
        [SerializeField] private float _shootRate;
        [SerializeField] private float _damage;

        public float ShootRange => _shootRange;
        public float ShootRate => _shootRate;
        public float Damage => _damage;
    }
}