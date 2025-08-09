using System.Collections.Generic;
using Enemies;
using Pools;
using Spawn;
using UnityEngine;

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

        public List<Enemy> Enemies => _enemies;
        
        
        public void Initialize(IEnemySpawner spawner, IBulletPool bulletPool)
        {
            _spawner = spawner;
            _bulletPool = bulletPool;
        }

        public void OnLevelStart(Transform player)
        {
            _player = player;
            SpawnEnemy(new Vector3(2f, 1f, 0f));
            SpawnEnemy(new Vector3(6f, 1f, 1f));
            SpawnEnemy(new Vector3(-4f, 1f, 3f));
        }
        
        private void SpawnEnemy(Vector3 pos)
        {
            var enemy = _spawner.SpawnEnemy(EnemyType.Red, pos);

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