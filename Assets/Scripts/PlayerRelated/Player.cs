using System;
using Core;
using Factories;
using Particles;
using PlayerUI;
using Pools;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Player: MonoBehaviour
    {

        public event Action OnPlayerDie = default;
        
        private IWavesProvider _wavesProvider;
        private IParticlesPool _particlesPool;
        
        [SerializeField] private Movement _movement;
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Health _health;
        [SerializeField] private ClosestEnemyProvider closestEnemyProvider;
        [SerializeField] private RangeDrawerUI _rangeDrawerUI;
        [SerializeField] private HealthbarUI _healthbar;
        [SerializeField] private DestructionDataHolder _destructionData;
       
        [Space]
        [Header("Settings")]
        [SerializeField] private MovementSettings _movementSettings;
        [SerializeField] private ShootSettings _shootSettings;
        [SerializeField] private BulletConfigs _bulletConfigs;
        [SerializeField] private HealthSettings _healthSettings;
        [SerializeField] private RangeUISettings _rangeUISettings;
        [Space]
        [Header("Renderers")]
        [SerializeField] private MeshRenderer _rendererMain;
        [SerializeField] private MeshRenderer _rendererHand;
          

        private void OnValidate()
        {
            _movement = GetComponent<Movement>();
            _shooter = GetComponent<Shooter>();
            _health = GetComponent<Health>();
            _destructionData = GetComponent<DestructionDataHolder>();
            
            closestEnemyProvider = GetComponent<ClosestEnemyProvider>();
        }

        public void Initialize(IInputProvider inputProvider, IWavesProvider wavesProvider, IBulletPool bulletPool, IParticlesPool vfxPool)
        { 
            closestEnemyProvider.Initialize(wavesProvider);
            _movement.Initialize(inputProvider, _movementSettings);
            _health.Initialize(_healthSettings);
            _shooter.Initialize(closestEnemyProvider, _shootSettings, _bulletConfigs, bulletPool);
           _rangeDrawerUI.Initialize(_rangeUISettings, _shootSettings);
           
           _healthbar.Initialize(_health);
         
            _wavesProvider = wavesProvider;
            _particlesPool = vfxPool;

            _health.OnHealthDepleted += OnHealthDepleted;
        }

        public void Show(bool on)
        {
            _rendererMain.enabled = on;
            _rendererHand.enabled = on;
            _rangeDrawerUI.gameObject.SetActive(on);
            _healthbar.gameObject.SetActive(on);
            _shooter.SetActive(on);

        }

        public void Reset()
        {
            _health.Initialize(_healthSettings);
        }
        
        private void OnHealthDepleted()
        {
            OnPlayerDie?.Invoke();
        }
    }
}