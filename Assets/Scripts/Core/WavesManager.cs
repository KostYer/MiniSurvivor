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
            SpawnEnemy(new Vector3(2f, 1f, 0f));
            SpawnEnemy(new Vector3(6f, 1f, 1f));
            SpawnEnemy(new Vector3(-4f, 1f, 3f));
        }
        
        private void SpawnEnemy(Vector3 pos)
        {
            var enemy = _spawner.SpawnEnemy(EnemyType.Red, pos);
            enemy.Initialize();
            _enemies.Add(enemy);
        }
    }
}