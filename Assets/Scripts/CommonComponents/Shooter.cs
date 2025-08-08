using System.Collections;
using Factories;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Shooter: MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
     
        private ITargetProvider _targetProvider;
        private ShootSettings _shootSettings;
        private BulletFactory _bulletFactory = new BulletFactory();
        private BulletConfigs _bulletConfigs;

        private float _shootRate => _shootSettings.ShootRate;
        private float _searchInterval = .05f;
        private float _actualRate;

        public void Initialize(ITargetProvider targetProvider, ShootSettings shootSettings, BulletConfigs bulletConfigs)
        {
            _targetProvider = targetProvider;
            _shootSettings = shootSettings;
            _bulletConfigs = bulletConfigs;
            _actualRate = _searchInterval;
            
            StartCoroutine(ShootPeriodically());
        }

        private IEnumerator ShootPeriodically()
        {
            while (true)
            {
                TryShoot();
                yield return new WaitForSeconds(_actualRate);
            }
        }

        private void TryShoot()
        {
            if (_targetProvider.TryFindTarget(out var targetPos, _shootSettings.ShootRange))
            {
                var bullet = _bulletFactory.CreateBullet(_bulletConfigs, _shootPoint.position);
                var direction = (targetPos - _shootPoint.position).normalized;
                bullet.LaunchBullet(_bulletConfigs.Damage, _bulletConfigs.Speed, direction);
                _actualRate = _shootRate;
                return;
            }

            _actualRate = _searchInterval; // ensures instant shoot after near target loas and find
        }
    }
}