using System;
using Core;
using Factories;
using Particles;
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
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Health _health;
        [SerializeField] private EnemyMovementInput _enemyMovementInput;
        [SerializeField] private ProximityToPlayerProvider _proximityToPlayerProvider;
        [SerializeField] private BulletConfigs _bulletConfigs;
        [SerializeField] private ShootSettings _shootSettings;
        [SerializeField] private HealthSettings _healthSettings;
        [SerializeField] private MovementSettings _movementSettings;
        [SerializeField] private DestructionDataHolder _destructionData;
        
        private IInputProvider _enemyInput;
        private IParticlesPool _particlesPool;
        
        public EnemyType Type => _type;

        private void OnValidate()
        {
            _enemyMovementInput = GetComponent<EnemyMovementInput>();
            _movement = GetComponent<Movement>();
            _shooter = GetComponent<Shooter>();
            _health = GetComponent<Health>();
            _characterController = GetComponent<CharacterController>();
            _destructionData = GetComponent<DestructionDataHolder>();
            
            _characterController.enabled = false;
        }

        public void Initialize(Transform player, IBulletPool bulletPool, IParticlesPool particlesPool)
        {
            _enemyMovementInput.Initialize(_shootSettings, player);
            _movement.Initialize(_enemyMovementInput, _movementSettings);
            _proximityToPlayerProvider.Initialize(player);
            _shooter.Initialize(_proximityToPlayerProvider, _shootSettings,_bulletConfigs,bulletPool);
            _health.Initialize(_healthSettings);

            _particlesPool = particlesPool;
            _health.OnHealthDepleted += OnHealthDepleted;
        }

        private void OnHealthDepleted()
        {
            var yOffset = new Vector3(0f, _destructionData.YSpawnOffset, 0f);
            _particlesPool.Get(_destructionData.DieParticlesType, transform.position + yOffset, transform.rotation);
            OnEnemyDied?.Invoke(this);
        }
    }
}