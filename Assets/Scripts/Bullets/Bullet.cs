using System;
using System.Collections;
using Factories;
using Pools;
using UnityEngine;

namespace PlayerRelated
{
    [RequireComponent(typeof(Rigidbody))]   
    public class Bullet: MonoBehaviour, IBulletPoolable
    {
        public BulletType BulletType => _bulletType;

        [SerializeField] private BulletType _bulletType;
        [SerializeField] private Rigidbody _rb;

        private IBulletPool _pool;
        private float _damage;
        private float _speed;
        private float _lifetime;
        private bool _isAlive;

        private void OnValidate()
        {
          _rb = GetComponent<Rigidbody>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(_damage);
                OnDie();
            }
        }

        public void SetConfig(BulletConfigs config)
        {
            _speed = config.Speed;
            _damage = config.Damage;
            _lifetime = config.Lifetime;
            SetLayer(this.gameObject, config.CollisionMask);
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }

        public void SetPool(IBulletPool pool)
        {
            _pool = pool;
        }
        
        public void LaunchBullet(Vector3 direction)
        {
            if (_rb == null) return;
            _isAlive = true;
            transform.forward = direction;
            _rb.AddForce(direction * _speed, ForceMode.Impulse);

            StartCoroutine(DieAfterDelay(_lifetime));
        }
        
        private void SetLayer(GameObject obj, LayerMask mask)
        {
            int layerIndex = Mathf.RoundToInt(Mathf.Log(mask.value, 2));
            obj.layer = layerIndex;
        }

        private void OnDie()
        {
            if (!_isAlive) return;
            _isAlive = false;
            _pool.Release(this);
        }

        private IEnumerator DieAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            OnDie();
        }
    }
}