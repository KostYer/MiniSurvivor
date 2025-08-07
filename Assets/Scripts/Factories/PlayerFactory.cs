using PlayerRelated;
using UnityEngine;

namespace Core
{
    public interface IPlayerFactory
    {
        public Player CreatePlayer();
    }
    
    
    [CreateAssetMenu(fileName = "PlayerFactorySO", menuName = "Factories/PlayerFactory")]
    public class PlayerFactory : ScriptableObject, IPlayerFactory
    {
        [SerializeField] private GameObject  _playerPrefab;

        public Player CreatePlayer()
        {
            return Instantiate(_playerPrefab).GetComponent<Player>();
        } 
    }
}