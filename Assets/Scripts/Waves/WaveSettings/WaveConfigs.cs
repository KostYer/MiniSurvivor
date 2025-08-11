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
        [SerializeField] private Vector2 _spawnRangeMinMax = new(20f, 25f); // how far enemies are being spawned outside camera bounds
        [SerializeField] private Vector2 _spawnRangeMinMaxInitial = new(4f, 10f); // initial burst radius
        [SerializeField] private List<WaveData> _waveData = new();
        [SerializeField] private float _spawnRate = .7f;
        
        public Vector2 SpawnRangeMinMax => _spawnRangeMinMax;
        public Vector2 SpawnRangeMinMaxInitial => _spawnRangeMinMaxInitial;
        public List<WaveData> WaveData => _waveData;
        public float SpawRate => _spawnRate;
      
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
                _waveData[i].TimeToSpawnDebug = CalcTimeToSpawn(_waveData[i]);
            }
        }

        private float CalcTimeToSpawn(WaveData wave)
        {
            int enemiesInWave = 0;

            for (int i = 0; i < wave.WaveUnits.Length; i++)
            {
                enemiesInWave += wave.WaveUnits[i].Count;
            }

            return (enemiesInWave - wave.InitialSpawn) * _spawnRate;
        }
    }

    [Serializable]
    public class WaveData
    {
        [ReadOnly] public int WaveNumber;
        public int InitialSpawn = 3; // how many enemies is spawned upon start wave
        [ReadOnly] public float TimeToSpawnDebug; //appr time to spawn the wave
        public WaveDataUnit[] WaveUnits => _waveUnits;
        [SerializeField] private WaveDataUnit[] _waveUnits = new WaveDataUnit[3];

        public void Initialize()
        {
            var enemyTypes = Enum.GetValues(typeof(EnemyType))
                .Cast<EnemyType>()
                .Where(type => type != EnemyType.None)
                .OrderByDescending(type => type) 
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