using System;
using PlayerRelated;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUI
{
    public class HealthbarUI: MonoBehaviour
    {
        [SerializeField] private RectTransform _barRoot;
        [SerializeField] private Image _bar;
        
        private Transform _camera;

      public void Initialize(Health health)
      {
          health.OnHealthChanged += OnHealthChanged;
          _camera = Camera.main.transform;
      }

      private void OnHealthChanged(float ratio)
      {
          _bar.fillAmount = ratio;
      }

      private void Update()
      {
          _barRoot.LookAt(_camera);
      }
    }
}