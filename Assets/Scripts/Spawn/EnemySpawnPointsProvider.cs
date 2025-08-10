using UnityEngine;

namespace Spawn
{
    public class EnemySpawnPointsProvider: ISpawnPointProvider
    {
        public Vector3 GetSpawnPoint(Vector3 centerPoint, Vector2 rangeMinMax)
        {
            float distance = Random.Range(rangeMinMax.x, rangeMinMax.y);

            Vector2 randomDir2D = Random.insideUnitCircle.normalized;

            Vector3 offset = new Vector3(randomDir2D.x, 0f, randomDir2D.y) * distance;

            return centerPoint + offset;
        }
    }
}