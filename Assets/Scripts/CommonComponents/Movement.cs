using Core;
using Settings;
using UnityEngine;

namespace PlayerRelated
{
    public class Movement: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        private MovementSettings _movementSettings;
        private IInputProvider _inputProvider;
        
        private float _speed;
        private float _targetRotation;
        private float _rotationVelocity;
        private Camera _mainCamera;
      
        
        private void OnValidate()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Initialize(IInputProvider inputProvider, MovementSettings settings)
        {
            _movementSettings = settings;
            _inputProvider = inputProvider;
            _mainCamera = Camera.main;
            _characterController .enabled = true;
        }
 
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _movementSettings.MovementSpeed;
    
            if (_inputProvider.InputDirection == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _inputProvider.InputDirection.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _movementSettings.SpeedChangeRate);
                   
                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _speed = targetSpeed;
            /*_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;*/

            // normalise input direction
            Vector3 inputDirection = new Vector3(_inputProvider.InputDirection.x, 0.0f, _inputProvider.InputDirection.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_inputProvider.InputDirection != Vector2.zero)
            {
               _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                                
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _movementSettings.RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
           
            // move the player
            _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0f, -9.8f, 0f));
        }
        
        void OnGUI()
        {
            /*GUILayout.BeginArea(new Rect(10, 10, 200, 100), GUI.skin.box);
            GUILayout.Label($"targetSpeed: {_debugTargetSpeed}");
            GUILayout.Label($"_speed: {_speed:F2}");
            GUILayout.EndArea();*/
        }
    }
}