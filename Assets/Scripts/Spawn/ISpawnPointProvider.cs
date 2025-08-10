using UnityEngine;

namespace Spawn
{
    public interface ISpawnPointProvider
    {
        Vector3 GetSpawnPoint(Vector3 centerPoint, Vector2 rangeMinMax);
    }
}