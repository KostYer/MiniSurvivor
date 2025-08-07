using Core;
using PlayerRelated;
using UnityEngine;

namespace Spawn
{
    public interface IPlayerSpawner
    {
        public Player SpawnPlayer();
    }

    public class PlayerSpawner: MonoBehaviour, IPlayerSpawner
    {
        private IPlayerFactory _playerFactory;
        
        [SerializeField] private Vector3 _spawnPos;
        
        public void Initialize(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public Player SpawnPlayer()
        {
            var player = _playerFactory.CreatePlayer();
            player.transform.position = _spawnPos;
            return player;
        }
    }
}