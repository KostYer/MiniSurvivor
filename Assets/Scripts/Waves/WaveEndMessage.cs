using System.Collections.Generic;
using Enemies;

namespace Waves
{
    public class WaveEndMessage
    {
        public bool IsVictory;
        public Dictionary<EnemyType, WaveStatsUnit> Stats;
        public float Timer;
    }
}