using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Waves;

namespace UI
{
    public class MainMenuUIController: MonoBehaviour
    {
        public event Action OnStartPressed;
        public event Action OnWaveEndConfirmed;

        [SerializeField] private CanvasGroup _mainMenuCanvas;
        [SerializeField] private CanvasGroup _joystickCanvas;
        [SerializeField] private CanvasGroup _wavesCanves;
        [SerializeField] private CanvasGroup _timerCanvas;
        [SerializeField] private StatsScreen _statsScreen;
        [SerializeField] private TimerUI _timer;

        public TimerUI Timer => _timer;

        private void Awake()
        {
       //     ShowCanvasGroup(_mainMenuCanvas, true);
       //     ShowCanvasGroup(_joystickCanvas, false);
        }

        public void StartButtonPressed()
        {
            OnStartPressed?.Invoke();
        }

        private void ShowCanvasGroup(CanvasGroup cv, bool show)
        {
            cv.alpha = show? 1 : 0;
            cv.interactable = show;
            cv.blocksRaycasts = show;
        }

        public void WaveConfirmClick()
        {
            OnWaveEndConfirmed?.Invoke();
            ShowCanvasGroup(_wavesCanves, false);
            ShowCanvasGroup(_joystickCanvas, true);
            ShowCanvasGroup(_joystickCanvas, true);
            ShowCanvasGroup(_timerCanvas, true);
        }

        public void OnWaveDefeated(Dictionary<EnemyType, WaveStatsUnit> stats)
        {
            _statsScreen.Initialize(stats);
            ShowCanvasGroup(_wavesCanves, true);
            ShowCanvasGroup(_joystickCanvas, false);
            ShowCanvasGroup(_timerCanvas, false);
        }
    }
}