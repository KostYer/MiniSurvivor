using System;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Health: MonoBehaviour
    {
        public event Action OnHealthDepleted = default;
        
        private float _maxHealth;
        private float _health;

        private float HealthValue => _health;

        public void Initialize(HealthSettings settings)
        {
            _maxHealth = settings.MaxHealth;
            _health = settings.MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0f)
            {
                OnHealthDepleted?.Invoke();
            }
        }
    }
}