using UnityEngine;

namespace Core
{
    public class PrefabSpawner
    {
        public T Spawn<T>(T prefab, Vector3 position) where T : MonoBehaviour
        {
            return GameObject.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}