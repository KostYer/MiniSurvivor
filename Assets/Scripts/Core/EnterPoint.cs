using System;
using Core;
using Spawn;
using UI;
using UnityEngine;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager = default;
    [SerializeField] private InputProvider _inputProvider = default;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private MainMenuUIController _uiController;


  private void OnValidate()
  {
      _gameManager   = GetComponent<GameManager>();
      _inputProvider = GetComponent<InputProvider>();
      _playerSpawner = GetComponent<PlayerSpawner>();
  }

  private void Awake()
    {
        BuildGraph();
    }

    private void BuildGraph()
    {
        _playerSpawner.Initialize(_playerFactory);
        _gameManager.Initialize(_inputProvider, _playerSpawner, _uiController);
        
    }
}
