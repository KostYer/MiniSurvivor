using Core;
using Factories;
using PlayerRelated;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class Enemy: MonoBehaviour
    {
        [SerializeField] private EnemyType  _type;
        [SerializeField] private Movement _movement;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private EnemyMovementInput enemyMovementInput;
        [SerializeField] private ProximityToPlayerProvider _proximityToPlayerProvider;
        [SerializeField] private BulletConfigs _bulletConfigs;
        [SerializeField] private ShootSettings _shootSettings;
        [SerializeField] private HealthSettings _healthSettings;
        [SerializeField] private MovementSettings _movementSettings;
        
        private IInputProvider _enemyInput;
        public EnemyType Type => _type;

        private void OnValidate()
        {
            enemyMovementInput = GetComponent<EnemyMovementInput>();
            _movement = GetComponent<Movement>();
            _shooter = GetComponent<Shooter>();
        }

        public void Initialize(Transform player)
        {
            enemyMovementInput.Initialize(_shootSettings, player);
            _movement.Initialize(enemyMovementInput, _movementSettings);
            _proximityToPlayerProvider.Initialize(player);
            _shooter.Initialize(_proximityToPlayerProvider, _shootSettings,_bulletConfigs);
            
        }
    }
}