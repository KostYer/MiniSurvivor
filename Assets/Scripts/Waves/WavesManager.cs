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
        private List<Enemy> _enemies = new();
        private IEnemySpawner _spawner;
        private Transform _player;
        private IBulletPool _bulletPool;
        private ISpawnPointProvider _pointProvider;
        private WaveStatistics _waveStatistics;
        private SpawnTypeProvider _spawnTypeProvider;

        private WaveConfigs _waveConfigs;
        private int _currentWave = 0;

        private float _spawnRate = 1.3f;
        

        public List<Enemy> Enemies => _enemies;
        
        
        public void Initialize(WaveConfigs waveConfigs, IEnemySpawner spawner, ISpawnPointProvider pointsProvider, IBulletPool bulletPool)
        {
            _waveConfigs = waveConfigs;
            _spawner = spawner;
            _pointProvider = pointsProvider;
            _bulletPool = bulletPool;
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
        }

        public void StartWave()
        {
            _currentWave++;
            var currentWaveConfig = _waveConfigs.WaveData.FirstOrDefault(p => p.WaveNumber == _currentWave);

            _waveStatistics = new WaveStatistics(currentWaveConfig.WaveUnits, _currentWave);
            _spawnTypeProvider = new SpawnTypeProvider(currentWaveConfig.WaveUnits, _currentWave);
            
            InitialBurstSpawn(currentWaveConfig);
            
            StartCoroutine(SpawnWave());
          
        }
        
        public void InitialBurstSpawn(WaveData waveConfigs)
        {
            if (waveConfigs == null)
            {
                Debug.LogError($"[WavesManager] configs for wave {_currentWave} not found");
                return;
            }

            var burst = GenerateInitialBurstData(waveConfigs);

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

        private Dictionary<EnemyType, int> GenerateInitialBurstData(WaveData waveData) // make initial spawn based on configs
        {
            int enemiesCount = 0;
            for (int i = 0; i < waveData.WaveUnits.Length; i++)
            {
                enemiesCount += waveData.WaveUnits[i].Count;
            }
            if (waveData.InitialSpawn > enemiesCount)
            {
                Debug.LogError($"[WavesManager] initialBurst is bigger than enemies in wave");
            }

            int remaining = waveData.InitialSpawn;
            var initBurst = new Dictionary<EnemyType, int>();
            
            for (int i = 0; i < waveData.WaveUnits.Length; i++)
            {
                var unit = new WaveDataUnit();
                unit.EnemyType = waveData.WaveUnits[i].EnemyType;
                
                int toTake = Mathf.Min(waveData.WaveUnits[i].Count, remaining);
                unit.Count = toTake;
                
                initBurst.Add(unit.EnemyType, unit.Count);
                remaining -= toTake;
                
                if(remaining <= 0) break;
            }

            return initBurst;
        }
    }
}