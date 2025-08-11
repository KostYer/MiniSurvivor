using System;
using System.Collections.Generic;
using Enemies;
using WaveSettings;

namespace Waves
{
    public class WaveSpawnUnit
    {
        public EnemyType EnemyType;
        public int CountInWave;
        public int CountSpawned;

        public bool IsTypeSpawnable => CountSpawned >= CountInWave;

        public WaveSpawnUnit(EnemyType et, int cnt, int spawned)
        {
            EnemyType = et;
            CountInWave = cnt;
            CountSpawned = spawned;
        }
    }
    
    public class SpawnTypeProvider
    {
        public event Action OnWaveSpawnOver = default;

        public bool IsSpawnActive;
        
        public Dictionary<EnemyType, WaveSpawnUnit> WaveSpawns => _waveSpawns;
        private Dictionary<EnemyType, WaveSpawnUnit> _waveSpawns = new();

        private List<EnemyType> _availableTypes = new();
        
        public int WaveNumber => _waveNumber;
        private int _waveNumber;
        
        public SpawnTypeProvider(WaveDataUnit[] waveUnits, int waveNum)
        {
            _waveNumber = waveNum;

            for (int i = 0; i < waveUnits.Length; i++)
            {
                _waveSpawns.Add(waveUnits[i].EnemyType, new WaveSpawnUnit(waveUnits[i].EnemyType, waveUnits[i].Count, 0));
                _availableTypes.Add(waveUnits[i].EnemyType);
            }

            IsSpawnActive = true;
        }

        public EnemyType GetAvailableType()
        {
            if (!IsSpawnActive) return EnemyType.None;
            if (_availableTypes.Count == 0) return EnemyType.None;

            return _availableTypes[UnityEngine.Random.Range(0, _availableTypes.Count - 1)];
        }


        public void OnEnemySpawned(EnemyType type)
        {
            _waveSpawns[type].CountSpawned++;

            if (_waveSpawns[type].IsTypeSpawnable)
            {
                _availableTypes.Remove(type);
            }

            if (_availableTypes.Count == 0) OnSpawnEnded(); 
        }


        private void OnSpawnEnded()
        {
            IsSpawnActive = false;
            OnWaveSpawnOver?.Invoke();
        }
    }
}