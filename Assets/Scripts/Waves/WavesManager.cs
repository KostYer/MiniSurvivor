using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Particles;
using Pools;
using Spawn;
using UnityEngine;
using Waves;
using WaveSettings;

namespace Core
{
    public interface IWavesProvider
    {
        List<Enemy> Enemies { get; }
    }
    public class WavesManager: MonoBehaviour, IWavesProvider
    {
        public event Action<WaveEndMessage> OnWaveCleared = default;
        
        private List<Enemy> _enemies = new();
        private IEnemySpawner _spawner;
        private Transform _player;
        private IBulletPool _bulletPool;
        private IParticlesPool _particlesPool;
        private ISpawnPointProvider _pointProvider;
        private WaveStatistics _waveStatistics = new();
        private SpawnTypeProvider _spawnTypeProvider;
        private InitialBurstProvider _initialBurstProvider = new InitialBurstProvider();

        private WaveConfigs _waveConfigs;
        private int _currentWave = 0;

        private float _spawnRate = .8f;
        private Coroutine _spawnCoroutine;
        

        public List<Enemy> Enemies => _enemies;
        public WaveStatistics WaveStatistics => _waveStatistics;
        
        
        public void Initialize(WaveConfigs waveConfigs, IEnemySpawner spawner, ISpawnPointProvider pointsProvider, IBulletPool bulletPool, IParticlesPool vfxPool)
        {
            _waveConfigs = waveConfigs;
            _spawner = spawner;
            _pointProvider = pointsProvider;
            _bulletPool = bulletPool;
            _spawnRate = waveConfigs.SpawRate;
            _particlesPool = vfxPool;
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
        }

        public void StartWave(int wave)
        {
            _currentWave = wave;
            var currentWaveConfig = _waveConfigs.WaveData.FirstOrDefault(p => p.WaveNumber == _currentWave);

            _waveStatistics.Initialize(currentWaveConfig.WaveUnits, _currentWave);
            _spawnTypeProvider = new SpawnTypeProvider(currentWaveConfig.WaveUnits, _currentWave);
            _waveStatistics.OnWaveCleared += OnWaveClear;     
            
            InitialBurstSpawn(currentWaveConfig);
            
            _spawnCoroutine = StartCoroutine(SpawnWave());
        }
 

        public void InitialBurstSpawn(WaveData waveConfigs)
        {
            if (waveConfigs == null)
            {
                Debug.LogError($"[WavesManager] configs for wave {_currentWave} not found");
                return;
            }

            var burst = _initialBurstProvider.GenerateInitialBurstData(waveConfigs);

            foreach (var key in burst.Keys)
            {
                if(burst[key] == 0) continue;

                for (int i = 0; i < burst[key]; i++)
                {
                    SpawnEnemy(key, _waveConfigs.SpawnRangeMinMaxInitial);
                }
            }
        }

        private IEnumerator SpawnWave()
        {
            while (_spawnTypeProvider.IsSpawnActive)
            {
                var type = _spawnTypeProvider.GetAvailableType();
                SpawnEnemy(type, _waveConfigs.SpawnRangeMinMax);
                
                yield return new WaitForSeconds(_spawnRate);
            }
        }

        private void SpawnEnemy(EnemyType type, Vector2 spawnRadius)
        {
            var pos = _pointProvider.GetSpawnPoint(_player.transform.position, spawnRadius);
            SpawnEnemy(type, pos);
        }

        private void SpawnEnemy(EnemyType type, Vector3 pos)
        {
            var enemy = _spawner.SpawnEnemy(type, pos);
            enemy.OnEnemyDied += OnEnemyDie;
            enemy.Initialize(_player.transform, _bulletPool, _particlesPool);
            _enemies.Add(enemy);
            _spawnTypeProvider.OnEnemySpawned(type);
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
            enemy.OnEnemyDied -= OnEnemyDie;
            _waveStatistics.OnEnemyDied(enemy.Type);
            Destroy(enemy.gameObject);
        }
        
        private void OnWaveClear()
        {
             _waveStatistics.OnWaveCleared -= OnWaveClear;
             _spawnTypeProvider = null;
             SendEndMessage(true);
        }
        
        private void SendEndMessage(bool isWin)
        {
            var message = new WaveEndMessage();
            message.IsVictory = isWin;
            message.Stats = _waveStatistics.WaveStats;
            OnWaveCleared?.Invoke(message);
        }

        public void Deactivate()
        {
             StopCoroutine(_spawnCoroutine);
            _waveStatistics.OnWaveCleared -= OnWaveClear;
            _currentWave = 0;
            for (int i = _enemies.Count -1 ; i >= 0; i--)
            {
                OnEnemyDie(_enemies[i]);
            }
        }
    }
}