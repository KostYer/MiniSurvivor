using System;
using System.Collections.Generic;
using Cinemachine;
using Enemies;
using PlayerRelated;
using Pools;
using Spawn;
using UI;
using UnityEngine;
using Waves;

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
        private TimeCounter _timeCounter;
        
        public CinemachineVirtualCamera virtualCamera;
        
        public void Initialize(IInputProvider inputProvider, IPlayerSpawner playerSpawner, WavesManager wavesManager, MainMenuUIController uiController, IBulletPool bulletPool, TimeCounter timeCounter)
        {
            _inputProvider = inputProvider;
            _playerSpawner = playerSpawner;
            _wavesManager = wavesManager;
            _uiController = uiController;
            _bulletsPool = bulletPool;
            _timeCounter = timeCounter;

            _wavesManager.OnWaveCleared += OnWaveCleared;
            _uiController.OnWaveEndConfirmed += OnWaveEndConfirmed;

            _timeCounter.OnSecondPass += _uiController.Timer.OnTimerTick;
        }

        private void OnWaveEndConfirmed()
        {
            StartWave();
        }

        private void Start()
        {
            StartLevel();
        }

        private void StartLevel()
        {
            SpawnPlayer();
            _wavesManager.SetPlayer(_player.transform);
            StartWave();
        }
        
        private void StartWave()
        {
            _player.Show(true);
            _player.Reset();
            _wavesManager.StartWave();
            _timeCounter.StartTimer();
        }
        
        private void OnWaveCleared(Dictionary<EnemyType, WaveStatsUnit> stats)
        {
            _uiController.OnWaveDefeated(stats);
            _player.Show(false);
            _timeCounter.StopCounter();
        }
 
        private void WaveFailed()
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