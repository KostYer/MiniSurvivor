using System;
using Cinemachine;
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

        private void Start()
        {
            StartGame();
        }
        
        private void OnWaveEndConfirmed()
        {
            StartWave();
        }

        private void StartGame()
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
     
        private void SpawnPlayer()
        {
            _player = _playerSpawner.SpawnPlayer();
            _player.Initialize(_inputProvider, _wavesManager, _bulletsPool);
           
            virtualCamera.Follow = _player.transform;
            virtualCamera.LookAt =  _player.transform;

            _player.OnPlayerDie += OnPlayerDie;
        }

        private void OnPlayerDie()
        {
            var message = new WaveEndMessage();
            message.Stats = _wavesManager.WaveStatistics.WaveStats;
            SendStats(message);
            InvokeGameOver();
        }
           
        private void OnWaveCleared(WaveEndMessage message)
        {
            SendStats(message);
            _player.Show(false);
            _timeCounter.StopCounter();
        }

        private void SendStats(WaveEndMessage message)
        {
            message.Timer = _timeCounter.CurrentTimer;
            _uiController.OnWaveDefeated(message);
        }

        private void InvokeGameOver()
        {
            _wavesManager.Deactivate();
            _timeCounter.StopCounter();
            
            _player.Show(false);
        }
    }
}