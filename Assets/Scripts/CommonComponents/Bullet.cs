using System;
using UnityEngine;

namespace PlayerRelated
{
    [RequireComponent(typeof(Rigidbody))]   
    public class Bullet: MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        private float _damage;

        private void OnValidate()
        {
          _rb = GetComponent<Rigidbody>();
        }

        public void LaunchBullet(float dmg, float speed, Vector3 direction)
        {
           
            if (_rb == null) return;
            
            _damage = dmg;
            transform.forward = direction;
            _rb.AddForce(direction * speed, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(_damage);
                Destroy(gameObject);
            }
            
        }
    }
}