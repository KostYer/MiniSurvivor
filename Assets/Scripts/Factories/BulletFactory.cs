using PlayerRelated;
using UnityEngine;

namespace Factories
{
    public class BulletFactory 
    {
        public Bullet CreateBullet(BulletConfigs config, Vector3 pos, bool setLayer = true)
        {
            var bullet = Object.Instantiate(config.Prefab, pos, Quaternion.identity).GetComponent<Bullet>();
            
            if (setLayer)
            {
                SetLayer(bullet.gameObject, config.CollisionMask);
            }
            return bullet;
           
        }
        
        public void SetLayer(GameObject obj, LayerMask mask)
        {
            int layerIndex = Mathf.RoundToInt(Mathf.Log(mask.value, 2));
            obj.layer = layerIndex;
        }
    }
}