using Core;
using Particles;
using Pools;
using Spawn;
using UI;
using UnityEngine;
using WaveSettings;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager = default;
    [SerializeField] private InputProvider _inputProvider = default;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private WaveConfigs _waveConfigs;
    [SerializeField] private MainMenuUIController _uiController;
    [SerializeField] private WavesManager _wavesManager;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private ParticlesPool _particlesPool;
    [SerializeField] private TimeCounter _timeCounter;
    private ISpawnPointProvider _enemySpawnPointProvider = new EnemySpawnPointsProvider();

      private void OnValidate()
      {
          _gameManager   = GetComponent<GameManager>();
          _inputProvider = GetComponent<InputProvider>();
          _playerSpawner = GetComponent<PlayerSpawner>();
          _bulletPool = GetComponent<BulletPool>();
          _timeCounter = GetComponent<TimeCounter>();
          _particlesPool = GetComponent<ParticlesPool>();
      }

    private void Awake()
    {
        ActualizeScriptableSettings();
        
        BuildGraph();
    }

    private void BuildGraph()
    {
        _uiController.Initialize();
        _particlesPool.Initialize();
        _bulletPool.Initialize(_gameManager);
        _playerSpawner.Initialize(_playerFactory);
        _enemySpawner.Initialize(_enemyFactory);
        _wavesManager.Initialize(_waveConfigs, _enemySpawner, _enemySpawnPointProvider, _bulletPool, _particlesPool);
        _gameManager.Initialize(_inputProvider, _playerSpawner, _wavesManager, _uiController, _bulletPool, _timeCounter, _particlesPool);
    }

    private void ActualizeScriptableSettings()
    {
        _waveConfigs.Initialize();
        _enemyFactory.Initialize();
    }
}
