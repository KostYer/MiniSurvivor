using System;
using UnityEngine;

namespace UI
{
    public class MainMenuUIController: MonoBehaviour
    {
        public event Action OnStartPressed;

        [SerializeField] private CanvasGroup _mainMenuCanvas;
        [SerializeField] private CanvasGroup _joystickCanvas;

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
        }
    }
}