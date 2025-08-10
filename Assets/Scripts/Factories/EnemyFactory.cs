using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Core
{
    public interface IEnemyFactory
    {
        public Enemy CreateEnemy(EnemyType enemyType);
    }

    [CreateAssetMenu(fileName = "EnemyFactorySO", menuName = "Factories/EnemyFactory")]
    public class EnemyFactory: ScriptableObject, IEnemyFactory
    {
        [SerializeField] private List<Enemy> _enemies = new();
        private Dictionary<EnemyType, Enemy> _enemiesDictionary = new();

        private void OnValidate()
        {
            if(_enemies.Count == 0) return;
            
            for (int i = 0; i < _enemies.Count; i++)
            {
                if(_enemiesDictionary.ContainsKey(_enemies[i].Type)) continue;
                _enemiesDictionary.Add(_enemies[i].Type, _enemies[i]);
            }
        }


        public Enemy CreateEnemy(EnemyType enemyType)
        {
            if (_enemiesDictionary.TryGetValue(enemyType, out var enemy))
            {
                return Instantiate(enemy);
            }

            Debug.LogWarning($"[EnemyFactory] enemyType {enemyType} is absent");
            return null;
        }
    }
}