using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using WaveSettings;

namespace Waves
{
    public class WaveStatsUnit
    {
        public EnemyType EnemyType;
        public int CountInWave;
        public int CountKilled;

        public bool IsTypeKilled => CountKilled >= CountInWave;

        public WaveStatsUnit(EnemyType et, int cnt, int killed)
        {
            EnemyType = et;
            CountInWave = cnt;
            CountKilled = killed;
        }
    }

    public class WaveStatistics
    {
        public event Action<Dictionary<EnemyType, WaveStatsUnit>> OnWaveKilled = default;

        private Dictionary<EnemyType, WaveStatsUnit> _gameStats = new(); //accumulutive storer 
        
        public Dictionary<EnemyType, WaveStatsUnit> WaveStats => _waveStats;
        private Dictionary<EnemyType, WaveStatsUnit> _waveStats = new();
        
        public int WaveNumber => _waveNumber;
        private int _waveNumber;
        
        public void Initialize(WaveDataUnit[] waveUnits, int waveNum)
        {
            _waveNumber = waveNum;
            _waveStats.Clear();

            for (int i = 0; i < waveUnits.Length; i++)
            {
                _waveStats.Add(waveUnits[i].EnemyType, new WaveStatsUnit(waveUnits[i].EnemyType, waveUnits[i].Count, 0));
            }
        }

        public void OnEnemyDied(EnemyType enemyType)
        {
            _waveStats[enemyType].CountKilled ++;
            CheckIfWaveKilled();
        }

        private void CheckIfWaveKilled()
        {
            foreach (var entry in _waveStats)
            {
                var key = entry.Key;
                 if (!_waveStats[key].IsTypeKilled)  return;
            }

            AfterWaveCleared();
        }

        private void AfterWaveCleared()
        {
            foreach (var kvp in _waveStats)
            {
                if (_gameStats.ContainsKey(kvp.Key))
                {
                    _gameStats[kvp.Key].CountKilled += kvp.Value.CountKilled;
                }
                else
                {
                    _gameStats[kvp.Key] = kvp.Value;
                }
            }
            
            OnWaveKilled?.Invoke(_waveStats);
            Debug.Log($"[WaveStatistics] the wave is died");
        }
    }
}