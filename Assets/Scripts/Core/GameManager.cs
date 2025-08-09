using System;
using Cinemachine;
using PlayerRelated;
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
        
        public CinemachineVirtualCamera virtualCamera;
        
        public void Initialize(IInputProvider inputProvider, IPlayerSpawner playerSpawner, WavesManager wavesManager, MainMenuUIController uiController)
        {
            _inputProvider = inputProvider;
            _playerSpawner = playerSpawner;
            _wavesManager = wavesManager;
            _uiController = uiController;
        }

        private void Start()
        {
            StartLevel();
        }

        private void StartLevel()
        {
            SpawnPlayer();
            _wavesManager.OnLevelStart(_player.transform);
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
            _player.Initialize(_inputProvider, _wavesManager);
           
            virtualCamera.Follow = _player.transform;
            virtualCamera.LookAt =  _player.transform;
        }

     
    }
}