using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ShootSettingsSO", menuName = "Settings/ShootSettings")]
    public class ShootSettings : ScriptableObject
    {
        [SerializeField] private float _shootRange;
        [SerializeField] private float _shootRate;
     //   [SerializeField] private float _damage;
        [SerializeField] private float _stopRange;

        public float ShootRange => _shootRange;
        public float ShootRate => _shootRate;
     //   public float Damage => _damage;
        public float StopRange => _stopRange;
    }
}