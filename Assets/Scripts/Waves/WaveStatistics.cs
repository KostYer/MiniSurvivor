using System;
using System.Collections.Generic;
using Enemies;
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
        public event Action OnWaveKilled = default;
        
        public Dictionary<EnemyType, WaveStatsUnit> WaveStats => _waveStats;
        private Dictionary<EnemyType, WaveStatsUnit> _waveStats = new();
        
        public int WaveNumber => _waveNumber;
        private int _waveNumber;
        
        public WaveStatistics(WaveDataUnit[] waveUnits, int waveNum)
        {
            _waveNumber = waveNum;

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
            
            OnWaveKilled?.Invoke();
            /*Debug.Log($"[WaveStatistics] the wave is died");
            
            foreach (var e in _waveStats)
            {
                Debug.Log($"[WaveStatistics] enemy type: {e.Key}, cnt in wave: {e.Value.CountInWave}, cnt killed: {e.Value.CountKilled}");
            }*/
        }
    }
}