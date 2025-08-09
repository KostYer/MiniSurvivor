using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Unity.Collections;
using UnityEngine;

namespace WaveSettings
{
    [CreateAssetMenu(fileName = "WaveConfigsSO", menuName = "Settings/WaveConfigs")]
    public class WaveConfigs: ScriptableObject
    {
        public List<WaveData> _waveData = new();

        private void OnValidate()
        {
            foreach (var wave in _waveData)
            {
                wave?.Initialize();
            }

            for (int i = 0; i < _waveData.Count; i++)
            {
                _waveData[i].WaveNumber = i + 1;
                _waveData[i]?.Initialize();
            }
        }
    }

    [Serializable]
    public class WaveData
    {
        [ReadOnly]
        public int WaveNumber;
        [SerializeField] private WaveDataUnit[] _waveUnits = new WaveDataUnit[3];

        public void Initialize()
        {
            var enemyTypes = Enum.GetValues(typeof(EnemyType))
                .Cast<EnemyType>()
                .Where(type => type != EnemyType.None)
                .ToArray();

            if (_waveUnits == null || _waveUnits.Length != enemyTypes.Length)
            {
                _waveUnits = new WaveDataUnit[enemyTypes.Length];
            }

            for (int i = 0; i < enemyTypes.Length; i++)
            {
                if (_waveUnits[i] == null)
                    _waveUnits[i] = new WaveDataUnit();

                _waveUnits[i].EnemyType = enemyTypes[i];
            }
        }
    }
    
    [Serializable]
    public class WaveDataUnit
    {
        public EnemyType EnemyType;
        public int Count;
    }
}