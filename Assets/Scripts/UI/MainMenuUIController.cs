using System;
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
        [SerializeField] private CanvasGroup _waveCanves;
        [SerializeField] private StatsScreen _statsScreen;
        [SerializeField] private TimerUI _timer;
        [SerializeField] private WaveNumUI _waveNumUI;

        
        public TimerUI Timer => _timer;
        public WaveNumUI WaveNumUI => _waveNumUI;

        private void Awake()
        {
       //     ShowCanvasGroup(_mainMenuCanvas, true);
       //     ShowCanvasGroup(_joystickCanvas, false);
        }

        public void StartButtonPressed()
        {
            ShowStatsScreen(false);
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
            ShowStatsScreen(false);
        }

        public void OnWaveDefeated(WaveEndMessage message)
        {
            _statsScreen.Initialize(message);
            ShowStatsScreen(true);
        }

        private void ShowStatsScreen(bool show)
        {
            ShowCanvasGroup(_wavesCanves, show);
            ShowCanvasGroup(_joystickCanvas, !show);
            ShowCanvasGroup(_timerCanvas, !show);
            ShowCanvasGroup(_waveCanves, !show);
        }
    }
}