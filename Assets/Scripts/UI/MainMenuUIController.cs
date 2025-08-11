using System;
using System.Collections.Generic;
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
        [SerializeField] private CanvasGroup _statsCanvas;
        [SerializeField] private CanvasGroup _timerCanvas;
        [SerializeField] private CanvasGroup _waveNumCanvas;
        [SerializeField] private StatsScreen _statsScreen;
        [SerializeField] private TimerUI _timer;
        [SerializeField] private WaveNumUI _waveNumUI;

        private List<CanvasGroup> _allCanvasGroups = new();
        
        public TimerUI Timer => _timer;
        public WaveNumUI WaveNumUI => _waveNumUI;
 
        private void OnValidate()
        {
            _allCanvasGroups.Add(_mainMenuCanvas);
            _allCanvasGroups.Add(_joystickCanvas);
            _allCanvasGroups.Add(_statsCanvas);
            _allCanvasGroups.Add(_timerCanvas);
            _allCanvasGroups.Add(_waveNumCanvas);
        }
 

        public void ShowStartScreen()
        {
            HideAll();
            ShowCanvasGroup(_mainMenuCanvas, true);
        }

     
        public void OnWaveDefeated(WaveEndMessage message)
        {
            _statsScreen.Initialize(message);
            ShowStatsScreen(true);
        }

        private void ShowStatsScreen(bool show)
        {
         
            HideAll();
            ShowCanvasGroup(_statsCanvas, show);
        }

        private void ShowGameplayCanvases()
        {
            HideAll();
            ShowCanvasGroup(_joystickCanvas, true);
            ShowCanvasGroup(_timerCanvas, true);
            ShowCanvasGroup(_waveNumCanvas, true);
        }

        private void HideAll()
        {
            foreach (var canvas in _allCanvasGroups)
            {
                ShowCanvasGroup(canvas, false);
            }
        }
        
        private void ShowCanvasGroup(CanvasGroup cv, bool show)
        {
            cv.alpha = show? 1 : 0;
            cv.interactable = show;
            cv.blocksRaycasts = show;
        }

        #region ButtonCallbacks

        public void StartButtonPressed()
        {
            OnStartPressed?.Invoke();
            ShowGameplayCanvases();
        }

        public void WaveConfirmClick()
        {
            OnWaveEndConfirmed?.Invoke();
            ShowGameplayCanvases();
        }

        #endregion
    }
}