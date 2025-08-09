using System;
using Core;
using Factories;
using PlayerRelated;
using Pools;
using Settings;
using UnityEngine;

namespace Enemies
{
    public class Enemy: MonoBehaviour
    {
        public event Action<Enemy> OnEnemyDied = default;
        
        [SerializeField] private EnemyType  _type;
        [SerializeField] private Movement _movement;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Health _health;
        [SerializeField] private EnemyMovementInput _enemyMovementInput;
        [SerializeField] private ProximityToPlayerProvider _proximityToPlayerProvider;
        [SerializeField] private BulletConfigs _bulletConfigs;
        [SerializeField] private ShootSettings _shootSettings;
        [SerializeField] private HealthSettings _healthSettings;
        [SerializeField] private MovementSettings _movementSettings;
        
        private IInputProvider _enemyInput;
        public EnemyType Type => _type;

        private void OnValidate()
        {
            _enemyMovementInput = GetComponent<EnemyMovementInput>();
            _movement = GetComponent<Movement>();
            _shooter = GetComponent<Shooter>();
            _health = GetComponent<Health>();
        }

        public void Initialize(Transform player, IBulletPool bulletPool)
        {
            _enemyMovementInput.Initialize(_shootSettings, player);
            _movement.Initialize(_enemyMovementInput, _movementSettings);
            _proximityToPlayerProvider.Initialize(player);
            _shooter.Initialize(_proximityToPlayerProvider, _shootSettings,_bulletConfigs,bulletPool);
            _health.Initialize(_healthSettings);
      
            _health.OnHealthDepleted += OnHealthDepleted;
        }

        private void OnHealthDepleted()
        {
            OnEnemyDied?.Invoke(this);
        }
    }
}