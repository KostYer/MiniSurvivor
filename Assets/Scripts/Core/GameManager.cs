using System;
using Spawn;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private IInputProvider _inputProvider;
        private IPlayerSpawner _playerSpawner;
        private MainMenuUIController _uiController;
        
        public void Initialize(IInputProvider inputProvider, IPlayerSpawner playerSpawner, MainMenuUIController uiController)
        {
            _inputProvider = inputProvider;
            _playerSpawner = playerSpawner;
            _uiController = uiController;
        }

        private void Start()
        {
           var player = _playerSpawner.SpawnPlayer();
           player.Initialize(_inputProvider);
        }
    }
}