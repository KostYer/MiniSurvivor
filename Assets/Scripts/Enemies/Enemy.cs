using System;
using Core;
using Factories;
using PlayerRelated;
using UnityEngine;

namespace Enemies
{
    public class Enemy: MonoBehaviour
    {
        [SerializeField] private EnemyType  _type;
        private IInputProvider _enemyInput;
        [SerializeField] private Movement _movement;
        [SerializeField] private EnemyBrain _enemyBrain;
        [SerializeField] private BulletConfigs _bulletConfigs;

        public EnemyType Type => _type;


        private void OnValidate()
        {
            _enemyBrain = GetComponent<EnemyBrain>();
            _movement = GetComponent<Movement>();
        }

        public void Initialize()
        {
            _movement.Initialize(_enemyBrain);
        }
    }
}