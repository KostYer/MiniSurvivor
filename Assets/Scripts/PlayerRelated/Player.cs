using Core;
using UnityEngine;

namespace PlayerRelated
{
    public class Player: MonoBehaviour
    {
       
        [SerializeField] private Movement _movement;

        private void OnValidate()
        {
            _movement = GetComponent<Movement>();
        }

        public void Initialize(IInputProvider inputProvider)
        {
            _movement.InputProvider = inputProvider;
        }
    }
}