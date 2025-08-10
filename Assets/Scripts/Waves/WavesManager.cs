using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
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
        public event Action<Dictionary<EnemyType, WaveStatsUnit>> OnWaveCleared = default;
        
        private List<Enemy> _enemies = new();
        private IEnemySpawner _spawner;
        private Transform _player;
        private IBulletPool _bulletPool;
        private ISpawnPointProvider _pointProvider;
        private WaveStatistics _waveStatistics = new();
        private SpawnTypeProvider _spawnTypeProvider;
        private InitialBurstProvider _initialBurstProvider = new InitialBurstProvider();

        private WaveConfigs _waveConfigs;
        private int _currentWave = 0;

        private float _spawnRate = .8f;
        

        public List<Enemy> Enemies => _enemies;
        
        
        public void Initialize(WaveConfigs waveConfigs, IEnemySpawner spawner, ISpawnPointProvider pointsProvider, IBulletPool bulletPool)
        {
            _waveConfigs = waveConfigs;
            _spawner = spawner;
            _pointProvider = pointsProvider;
            _bulletPool = bulletPool;
            _spawnRate = waveConfigs.SpawRate;
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
        }

      
        public void StartWave()
        {
            _currentWave++;
            var currentWaveConfig = _waveConfigs.WaveData.FirstOrDefault(p => p.WaveNumber == _currentWave);

            _waveStatistics.Initialize(currentWaveConfig.WaveUnits, _currentWave);
            _spawnTypeProvider = new SpawnTypeProvider(currentWaveConfig.WaveUnits, _currentWave);
            _waveStatistics.OnWaveKilled += OnWaveDestroyed;     
            
            InitialBurstSpawn(currentWaveConfig);
            
            StartCoroutine(SpawnWave());
        }


        private void OnSpawnOver()
        {
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
            var pos = _pointProvider.GetSpawnPoint(_player.position, spawnRadius);
            SpawnEnemy(type, pos);
        }

        private void SpawnEnemy(EnemyType type, Vector3 pos)
        {
            var enemy = _spawner.SpawnEnemy(type, pos);
            enemy.OnEnemyDied += OnEnemyDie;
            enemy.Initialize(_player, _bulletPool);
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
        
        
        private void OnWaveDestroyed(Dictionary<EnemyType, WaveStatsUnit> stats)
        {
             Debug.Log($"[OnWaveDestroyed]");
             _waveStatistics.OnWaveKilled -= OnWaveDestroyed;
             _spawnTypeProvider = null;
             OnWaveCleared?.Invoke(stats);
        }
    }
}