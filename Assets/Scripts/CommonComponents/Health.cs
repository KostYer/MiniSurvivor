using System;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Health: MonoBehaviour
    {
        public event Action OnHealthDepleted = default;
        public event Action<float> OnHealthChanged = default;
        
        private float _maxHealth;
        private float _health;
        private bool _IsAlive;

        private float HealthValue => _health;

        public void Initialize(HealthSettings settings)
        {
            _maxHealth = settings.MaxHealth;
            _health = settings.MaxHealth;
            OnHealthChanged?.Invoke(_health/_maxHealth);
            _IsAlive = true;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            OnHealthChanged?.Invoke(_health/_maxHealth);
            if (_health <= 0f && _IsAlive)
            {
                _IsAlive = false;
                OnHealthDepleted?.Invoke();
            }
        }
    }
}