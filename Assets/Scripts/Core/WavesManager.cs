using System.Collections.Generic;
using Enemies;
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

        public List<Enemy> Enemies => _enemies;
        
        
        public void Initialize(IEnemySpawner spawner)
        {
            _spawner = spawner;
        }

        public void OnLevelStart()
        {
            SpawnEnemy();
        }
        
        private void SpawnEnemy()
        {
            var enemy = _spawner.SpawnEnemy(EnemyType.Red, new Vector3(2f, 1f, 0f));
            enemy.Initialize();
            _enemies.Add(enemy);
        }
    }
}