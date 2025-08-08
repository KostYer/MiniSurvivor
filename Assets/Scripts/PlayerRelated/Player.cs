using System;
using Core;
using Factories;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Player: MonoBehaviour
    {
        private IWavesProvider _wavesProvider;
        
        [SerializeField] private Movement _movement;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Health _health;
        [SerializeField] private PlayerTargetProvider _playerTargetProvider;
       
        [Space]
        [Header("Settings")]
        [SerializeField] private MovementSettings _movementSettings;
        [SerializeField] private ShootSettings _shootSettings;
        [SerializeField] private BulletConfigs _bulletConfigs;
        [SerializeField] private HealthSettings _healthSettings;
          

        private void OnValidate()
        {
            _movement = GetComponent<Movement>();
            _shooter = GetComponent<Shooter>();
            _health = GetComponent<Health>();
            _playerTargetProvider = GetComponent<PlayerTargetProvider>();
        }

        public void Initialize(IInputProvider inputProvider, IWavesProvider wavesProvider)
        { 
            _playerTargetProvider.Initialize(wavesProvider);
            _movement.Initialize(inputProvider, _movementSettings);
            _health.Initialize(_healthSettings);
            _shooter.Initialize(_playerTargetProvider, _shootSettings, _bulletConfigs);
            
         
            _wavesProvider = wavesProvider;
        }
    }
}