using Core;
using Pools;
using Spawn;
using UI;
using UnityEngine;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager = default;
    [SerializeField] private InputProvider _inputProvider = default;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private MainMenuUIController _uiController;
    [SerializeField] private WavesManager _wavesManager;
    [SerializeField] private BulletPool _bulletPool;

      private void OnValidate()
      {
          _gameManager   = GetComponent<GameManager>();
          _inputProvider = GetComponent<InputProvider>();
          _playerSpawner = GetComponent<PlayerSpawner>();
          _bulletPool = GetComponent<BulletPool>();
      }

    private void Awake()
    {
        BuildGraph();
    }

    private void BuildGraph()
    {
        _bulletPool.Initialize();
        _playerSpawner.Initialize(_playerFactory);
        _enemySpawner.Initialize(_enemyFactory);
        _wavesManager.Initialize(_enemySpawner, _bulletPool);
        _gameManager.Initialize(_inputProvider, _playerSpawner, _wavesManager, _uiController, _bulletPool);
    }
}
