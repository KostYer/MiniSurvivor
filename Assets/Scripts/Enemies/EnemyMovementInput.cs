using System;
using System.Collections;
using Core;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyMovementInput : MonoBehaviour, IInputProvider
    {
        private Vector2 _input = Vector2.zero;
        public Vector2 InputDirection => _input;

        private Transform _player;
        private ShootSettings _shootSettings;

        private float _updDirRate = .02f;
        private float _approachAngleOffset;
        private bool _isActive;

        public void Initialize(ShootSettings shootSettings, Transform player)
        {
            _shootSettings = shootSettings;
            _player = player;
            _isActive = true;
            
            _approachAngleOffset = Random.Range(-35f, 35f);
            _input = Vector2.zero;
            StartCoroutine(UpdateDirectionCor(_updDirRate));
        }

        private IEnumerator UpdateDirectionCor(float rate)
        {
            while (_isActive)
            {
                UpdDirection();
                yield return new WaitForSeconds(rate);
            }
        }

        private void UpdDirection()
        {
           
            var direction = _player.position - transform.position;
            direction = new Vector2(direction.x, direction.z);
            
            direction = Quaternion.Euler(0, 0, _approachAngleOffset) * direction;

            float distance = direction.magnitude;

            float stopRange = _shootSettings.ShootRange;

            if (distance <= stopRange)
            {
                // Inside range → stop completely
                _input = Vector2.zero;
            }
            else if (distance <= stopRange + 1f) // 1f = braking distance
            {
                // Just outside range → slow down proportionally
                float t = Mathf.InverseLerp(stopRange + 1f, stopRange, distance);
                _input = direction.normalized * (1f - t);
            }
            else
            {
                // Far from target → move full speed
                _input = direction.normalized;
            }
        }
    }
}