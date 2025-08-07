using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "MovementSettingsSO", menuName = "Settings/MovementSettings")]
    public class MovementSettings: ScriptableObject
    {
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _speedChangeRate = 10f;
        [Range(0f, .3f)]
        [SerializeField] private float _rotationSmoothTime;
        
        public float MovementSpeed => _movementSpeed;
        public float SpeedChangeRate => _speedChangeRate;
        public float RotationSmoothTime => _rotationSmoothTime;
    }
}