using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "HealthSettingsSO", menuName = "Settings/HealthSettings")]
    public class HealthSettings: ScriptableObject
    {
        [SerializeField] private float _maxHealth;

        public float MaxHealth => _maxHealth;
    }
}