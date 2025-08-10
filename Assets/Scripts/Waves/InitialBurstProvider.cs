using System.Collections.Generic;
using Enemies;
using UnityEngine;
using WaveSettings;

namespace Waves
{
    public class InitialBurstProvider
    {
        public Dictionary<EnemyType, int> GenerateInitialBurstData(WaveData waveData) // make initial spawn based on configs
        {
            int enemiesCount = 0;
            for (int i = 0; i < waveData.WaveUnits.Length; i++)
            {
                enemiesCount += waveData.WaveUnits[i].Count;
            }
            if (waveData.InitialSpawn > enemiesCount)
            {
                Debug.LogError($"[WavesManager] initialBurst is bigger than enemies in wave");
            }

            int remaining = waveData.InitialSpawn;
            var initBurst = new Dictionary<EnemyType, int>();
            
            for (int i = 0; i < waveData.WaveUnits.Length; i++)
            {
                var unit = new WaveDataUnit();
                unit.EnemyType = waveData.WaveUnits[i].EnemyType;
                
                int toTake = Mathf.Min(waveData.WaveUnits[i].Count, remaining);
                unit.Count = toTake;
                
                initBurst.Add(unit.EnemyType, unit.Count);
                remaining -= toTake;
                
                if(remaining <= 0) break;
            }

            return initBurst;
        }
    }
}