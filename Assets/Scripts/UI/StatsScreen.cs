using System.Collections.Generic;
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
        
        [Header("Buttons")]
        [SerializeField] private CanvasGroup _nextWaveButton;
        [SerializeField] private CanvasGroup _startOverButton;


        public void Initialize(WaveEndMessage message)
        {
            _result.text = message.IsVictory ? "Victory" : "Defeat";
            _buttonText.text = message.IsVictory ? "Continue" : "Start Over";
            
            int i = 0;
            foreach (var kvp in message.Stats)
            {
                _enemyHeaders[i].text = "Enemy " + kvp.Value.EnemyType;
                _enemyCounters[i].text = kvp.Value.CountKilled.ToString();
                i++;
            }

            PickButton(message.IsVictory);
        }

        private void PickButton(bool isVictory)
        {
            ShowCanvasGroup(_nextWaveButton, isVictory);
            ShowCanvasGroup(_startOverButton, !isVictory);
        }
        
        private void ShowCanvasGroup(CanvasGroup cv, bool show)
        {
            cv.alpha = show? 1 : 0;
            cv.interactable = show;
            cv.blocksRaycasts = show;
        }
    }
}