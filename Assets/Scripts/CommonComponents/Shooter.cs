using System.Collections;
using Factories;
using Pools;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Shooter: MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private BulletType _bulletType;
     
        private ITargetProximityProvider _targetProximityProvider;
        private IBulletPool _bulletPool;
        private ShootSettings _shootSettings;
        private BulletConfigs _bulletConfigs;

        private float _shootRate => _shootSettings.ShootRate;
        private float _searchInterval = .05f;
        private float _actualRate;
        private bool _isActive = true;

        public void Initialize(ITargetProximityProvider targetProximityProvider, ShootSettings shootSettings, BulletConfigs bulletConfigs, IBulletPool bulletPool)
        {
            _targetProximityProvider = targetProximityProvider;
            _shootSettings = shootSettings;
            _bulletConfigs = bulletConfigs;
            _actualRate = _searchInterval;
            _bulletPool = bulletPool;
            
            StartCoroutine(ShootPeriodically());
        }

        private IEnumerator ShootPeriodically()
        {
            while (_isActive)
            {
                TryShoot();
                yield return new WaitForSeconds(_actualRate);
            }
        }

        private void TryShoot()
        {
            if (_targetProximityProvider.IsTargetClose(out var targetPos, _shootSettings.ShootRange))
            {
              //var bullet = _bulletFactory.CreateBullet(_bulletConfigs, _shootPoint.position);
               var bullet = _bulletPool.Get(_bulletType, _bulletConfigs, _shootPoint.position, Quaternion.identity);
                
                var direction = (targetPos - _shootPoint.position).normalized;
                bullet.LaunchBullet(direction);
                _actualRate = _shootRate;
                return;
            }

            _actualRate = _searchInterval; // ensures instant shoot after near target lost and found
        }

        public void SetActive(bool on)
        {
            _isActive = on;
        }
    }
}