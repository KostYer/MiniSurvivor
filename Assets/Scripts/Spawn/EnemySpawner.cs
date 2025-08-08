using Core;
using Enemies;
using UnityEngine;

namespace Spawn
{
    public interface IEnemySpawner
    {
        public Enemy SpawnEnemy(EnemyType enemyType, Vector3 pos);
    }
    public class EnemySpawner: MonoBehaviour, IEnemySpawner
    {
        private IEnemyFactory _enemyFactory;
        public void Initialize(IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
        }

        public Enemy SpawnEnemy(EnemyType enemyType,Vector3 pos)
        {
            var enemy = _enemyFactory.CreateEnemy(enemyType);
            enemy.transform.position = pos;
            return enemy;
        }
    }
}