using System;
using Cinemachine;
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
            _wavesManager.OnLevelStart();
        }

        private void LevelCleared()
        {
        }

        private void GameOver()
        {
        }

        private void SpawnPlayer()
        {
            var player = _playerSpawner.SpawnPlayer();
            player.Initialize(_inputProvider, _wavesManager);
           
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt =  player.transform;
        }

     
    }
}