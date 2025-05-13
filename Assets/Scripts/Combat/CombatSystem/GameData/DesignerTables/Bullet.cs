using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingerTables
{
    
    ///<summary>
    ///BulletModel
    ///</summary>
    public class Bullet{
        //OnCreate,参数：
        //OnHit,参数:[0]伤害倍数[1]基础暴击率[2]命中视觉特效Id[3]播放特效位于目标的绑点，默认Body[5]击中音效Id
        //OnHitObstacle,参数：
        //OnRemoved,参数：
        public static Dictionary<string, BulletModel> data = new Dictionary<string, BulletModel>(){
            {"DefaultBullet", new BulletModel(
                "DefaultBullet", 0, 50,501,
                "", new object[0],
                "CommonBulletHit", new object[]{1.0f,0.05f,0,"Body",550},
               
                "CommonBulletRemoved", new object[]{-1,-1}//[0]EffectId，[1]音效id
            ){isTrigger = false,useGravity = true,moveMethod = MoveMethod.RigidbodyVelocity,moveSpeedParam = 9999,hitTimes = 1}},
            
            {"FortBullet", new BulletModel(
                "FortBullet", 0, 50,501,
                "", new object[]{},
                "CommonBulletHit", new object[]{1.0f,0.05f,0,"Body",550},
              
                "", new object[]{}//[0]EffectId，[1]音效id
            ){isTrigger = true,useGravity = false,moveMethod = MoveMethod.RigidbodyVelocity,moveSpeedParam = 9999,hitTimes = 1}},
           
        };
    }
}