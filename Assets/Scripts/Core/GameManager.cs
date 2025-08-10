using System;
using Cinemachine;
using PlayerRelated;
using Pools;
using Spawn;
using UI;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public Action OnLevelStart = default;
        public Action OnLevelEnd = default;
        public Action OnGameOver = default;
        
        private IInputProvider _inputProvider;
        private IPlayerSpawner _playerSpawner;
        private IEnemySpawner _enemySpawner;
        private MainMenuUIController _uiController;
        private WavesManager _wavesManager;
        private Player _player;
        private IBulletPool _bulletsPool;
        
        public CinemachineVirtualCamera virtualCamera;
        
        public void Initialize(IInputProvider inputProvider, IPlayerSpawner playerSpawner, WavesManager wavesManager, MainMenuUIController uiController, IBulletPool bulletPool)
        {
            _inputProvider = inputProvider;
            _playerSpawner = playerSpawner;
            _wavesManager = wavesManager;
            _uiController = uiController;
            _bulletsPool = bulletPool;
        }

        private void Start()
        {
            StartLevel();
        }

        private void StartLevel()
        {
            SpawnPlayer();
            _wavesManager.SetPlayer(_player.transform);
            _wavesManager.StartWave();
           
        }
        
        private void StartWave()
        {
          
        }

        private void LevelCleared()
        {
        }

        private void GameOver()
        {
        }

        private void SpawnPlayer()
        {
            _player = _playerSpawner.SpawnPlayer();
            _player.Initialize(_inputProvider, _wavesManager, _bulletsPool);
           
            virtualCamera.Follow = _player.transform;
            virtualCamera.LookAt =  _player.transform;
        }
    }
}