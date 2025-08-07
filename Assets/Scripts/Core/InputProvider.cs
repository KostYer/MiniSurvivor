using UnityEngine;

namespace Core
{
    public interface IInputProvider
    {
        Vector2 InputDirection { get; }
    }

    public class InputProvider: MonoBehaviour, IInputProvider
    {
        [SerializeField] private Joystick _joystick;
        
        public Vector2 InputDirection => _joystick.Direction;
    }
}