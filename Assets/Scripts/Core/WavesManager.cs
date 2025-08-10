using System.Collections;
using System.Collections.Generic;
using Enemies;
using Pools;
using Spawn;
using UnityEngine;
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

        private WaveConfigs _waveConfigs;
        private int _currentWave = 0;

        private float _spawnRate = 1.3f;
        private bool _isWaveActive;

        public List<Enemy> Enemies => _enemies;
        
        
        public void Initialize(WaveConfigs waveConfigs, IEnemySpawner spawner, ISpawnPointProvider pointsProvider, IBulletPool bulletPool)
        {
            _waveConfigs = waveConfigs;
            _spawner = spawner;
            _pointProvider = pointsProvider;
            _bulletPool = bulletPool;
        }

        public void OnLevelStart(Transform player)
        {
            _player = player;

            StartWave();
          
        }

        private void StartWave()
        {
            _isWaveActive = true;

            StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            while (_isWaveActive)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(_spawnRate);
            }
        }

        private void SpawnEnemy()
        {
            var pos = _pointProvider.GetSpawnPoint(_player.position, new Vector2(25f, 30f));
            SpawnEnemy(pos);
         //   SpawnEnemy(new Vector3(6f, 1f, 1f));
      //      SpawnEnemy(new Vector3(-4f, 1f, 3f));
        }

        private void SpawnEnemy(Vector3 pos)
        {
            var enemy = _spawner.SpawnEnemy(EnemyType.Blue, pos);
            enemy.transform.position  = pos;
            Debug.Log($"[SpawnEnemy] distance to player {Vector3.Distance(enemy.transform.position, _player.transform.position)}");
            enemy.OnEnemyDied += OnEnemyDie;
            enemy.Initialize(_player, _bulletPool);
            _enemies.Add(enemy);
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemies.Remove(enemy);
            enemy.OnEnemyDied -= OnEnemyDie;
            Destroy(enemy.gameObject);
        }
        
        
    }
}