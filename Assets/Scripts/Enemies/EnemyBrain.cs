using Core;
using UnityEngine;

namespace Enemies
{
    public class EnemyBrain : MonoBehaviour, IInputProvider
    {
        private Vector2 _input = Vector2.zero;
        public Vector2 InputDirection => _input;
    }
}