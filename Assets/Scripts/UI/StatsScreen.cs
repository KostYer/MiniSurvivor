using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using Waves;

namespace UI
{
    public class StatsScreen: MonoBehaviour
    {
        [SerializeField] private TMP_Text _result;
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private List<TMP_Text> _enemyHeaders = new();
        [SerializeField] private List<TMP_Text> _enemyCounters = new();


        public void Initialize(Dictionary<EnemyType, WaveStatsUnit> stats)
        {
            int i = 0;
            foreach (var kvp in stats)
            {
                _enemyHeaders[i].text = "Enemy " + kvp.Value.EnemyType;
                _enemyCounters[i].text = kvp.Value.CountKilled.ToString();
                i++;
            }
        }
    }
}