using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class TimeCounter: MonoBehaviour
    {
        public event Action<float> OnSecondPass = default;
        
        private readonly float _tick = 1f;

        public float CurrentTimer => _currentTimer;
        
        private float _currentTimer;
        private bool _isActive;
      
        
        public void StartTimer()
        {
            _currentTimer = 0f;
            _isActive = true;
            OnSecondPass?.Invoke(_currentTimer);
            StartCoroutine(TimerCoroutine());
        }

        public void StopCounter()
        {
            _isActive = false;
            OnSecondPass?.Invoke(0f);
        }

        private IEnumerator TimerCoroutine()
        {
            while (_isActive)
            {
                yield return new WaitForSeconds(_tick);
                _currentTimer++;
                OnSecondPass?.Invoke(_currentTimer);
            }
            OnSecondPass?.Invoke(0f);
        }
    }
}