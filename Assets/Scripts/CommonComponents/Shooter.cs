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

        public void Initialize(ITargetProvider targetProvider, ShootSettings shootSettings, BulletConfigs bulletConfigs)
        {
            _targetProvider = targetProvider;
            _shootSettings = shootSettings;
            _bulletConfigs = bulletConfigs;
            StartCoroutine(ShootPeriodically(_shootSettings.ShootRate));
             
        }

        private IEnumerator ShootPeriodically(float interval)
        {
            while (true)
            {
                TryShoot();
                yield return new WaitForSeconds(interval);
            }
        }

        private void TryShoot()
        {
            if (_targetProvider.TryFindTarget(out var targetPos, 10f))
            {
                var bullet = _bulletFactory.CreateBullet(_bulletConfigs, _shootPoint.position);
                var direction = (targetPos - _shootPoint.position).normalized;
                bullet.LaunchBullet(_bulletConfigs.Damage, _bulletConfigs.Speed, direction);
            }
        }
    }
}