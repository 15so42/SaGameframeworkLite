using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DesignerScripts
{
    ///<summary>
    ///子弹的效果
    ///</summary>
    public class Bullet{
        //onCreate在第一次fxiedUpdate中执行
        public static Dictionary<string, BulletOnCreate> onCreateFunc = new Dictionary<string, BulletOnCreate>(){
            {"ResetRbAngularVelocity",ResetRbAngularVelocity},
        };
        public static Dictionary<string, BulletOnHit> onHitFunc = new Dictionary<string, BulletOnHit>(){
            {"CommonBulletHit", CommonBulletHit},
            {"CommonBulletHeal", CommonBulletHeal},
            {"AddHpOnHit", AddHpOnHit},
            {"CreateAoEOnHit", CreateAoEOnHit},//注意AoE，不是Aoe
            {"CloakBoomerangHit", CloakBoomerangHit},
            {"ExplosionOnHit", ExplosionOnHit}
        };
        
        public static Dictionary<string, BulletOnRemoved> onRemovedFunc = new Dictionary<string, BulletOnRemoved>(){
            {"CommonBulletRemoved", CommonBulletRemoved},
            {"CreateAoEOnRemoved", CreateAoEOnRemoved},
            {"ExplosionOnRemoved", ExplosionOnRemoved},
        };
        public static Dictionary<string, BulletTween> bulletTween = new Dictionary<string, BulletTween>(){
            {"FollowingTarget", FollowingTarget},
            {"SlowlyFaster", SlowlyFaster},//缓慢变快
            
          
        };
        
        public static Dictionary<string, BulletTweenCondition> bulletTweenCondition = new Dictionary<string, BulletTweenCondition>(){
            {"OnlyUseTweenOnFirstFrame", OnlyUseTweenOnFirstFrame},
           
            
          
        };
        public static Dictionary<string, BulletTargettingFunction> targettingFunc = new Dictionary<string, BulletTargettingFunction>(){
            {"GetNearestEnemy", GetNearestEnemy},
            {"BulletCasterSelf", BulletCasterSelf}
        };


        public static void ResetRbAngularVelocity(BulletState bullet)
        {
            bullet.transform.rotation = bullet.fireRotation;
            
        }

        ///<summary>
        ///onHit
        ///普通子弹命中效果，参数：
        ///[0]伤害倍数
        ///[1]基础暴击率
        ///[2]命中视觉特效Id
        ///[3]播放特效位于目标的绑点，默认Body
        ///[5]击中音效Id
        ///</summary>
        private static void CommonBulletHit(BulletState bullet, ChaState target){
            BulletState bulletState = bullet;
            if (!bulletState) return;
            object[] onHitParam = bulletState.model.onHitParams;
            float damageTimes = onHitParam.Length > 0 ? (float)onHitParam[0] : 1.00f;
            float critRate = onHitParam.Length > 1 ? (float) onHitParam[1] : 0.00f;
            int sightEffectId = onHitParam.Length > 2 ? (int) onHitParam[2] : -1;
            string bpName = onHitParam.Length > 3 ? (string) onHitParam[3] : "Body";
            int audioId= onHitParam.Length > 4 ? (int) onHitParam[4] :-1;
            if (sightEffectId >= 0){
                UnitBindManager ubm = target.GetComponent<UnitBindManager>();
                if (ubm){
                    Debug.Log($"播放受击特效，部位{bpName}");
                    ubm.AddBindEffect(bpName, sightEffectId, "", false);
                }
            }

            if (audioId >= 0)
            {
                GameEntry.Sound.PlaySFX3D(audioId,bullet.transform.position);
            }
            
            //Debug.Log($"CreateDamage");
            GameEntry.Combat.CreateDamage(
                bulletState.caster, 
                target.gameObject,
                new Damage(Mathf.CeilToInt(damageTimes * bulletState.propWhileCast.attack)),
                critRate,
                new DamageInfoTag[]{DamageInfoTag.directDamage, }
            );
        }
        
        
        ///<summary>
        ///onHit
        ///普通子弹命中效果，参数：
        ///[0]伤害倍数
        ///[1]基础暴击率
        ///[2]命中视觉特效Id
        ///[3]播放特效位于目标的绑点，默认Body
        ///[5]击中音效Id
        ///</summary>
        private static void CommonBulletHeal(BulletState bullet, ChaState target){
            BulletState bulletState = bullet;
            if (!bulletState) return;
            object[] onHitParam = bulletState.model.onHitParams;
            float damageTimes = onHitParam.Length > 0 ? (float)onHitParam[0] : 1.00f;
            float critRate = onHitParam.Length > 1 ? (float) onHitParam[1] : 0.00f;
            int sightEffectId = onHitParam.Length > 2 ? (int) onHitParam[2] : -1;
            string bpName = onHitParam.Length > 3 ? (string) onHitParam[3] : "Body";
            int audioId= onHitParam.Length > 4 ? (int) onHitParam[4] :-1;
            if (sightEffectId >= 0){
                UnitBindManager ubm = target.GetComponent<UnitBindManager>();
                if (ubm){
                    Debug.Log($"播放受击特效，部位{bpName}");
                    ubm.AddBindEffect(bpName, sightEffectId, "", false);
                }
            }

            if (audioId >= 0)
            {
                GameEntry.Sound.PlaySFX3D(audioId,bullet.transform.position);
            }
            
            GameEntry.Combat.CreateDamage(
                bulletState.caster, 
                target.gameObject,
                new Damage(Mathf.CeilToInt(-damageTimes * bulletState.propWhileCast.attack)),
                critRate,
                new DamageInfoTag[]{DamageInfoTag.directHeal, }
            );
        }
        
        private static void AddHpOnHit(BulletState bullet, ChaState target){
            BulletState bulletState = bullet;
            if (!bulletState) return;
            object[] onHitParam = bulletState.model.onHitParams;
            float damageTimes = onHitParam.Length > 0 ? (float)onHitParam[0] : 1.00f;
            float critRate = onHitParam.Length > 1 ? (float) onHitParam[1] : 0.00f;
            int sightEffectId = onHitParam.Length > 2 ? (int) onHitParam[2] : -1;
            string bpName = onHitParam.Length > 3 ? (string) onHitParam[3] : "Body";
            int audioId= onHitParam.Length > 4 ? (int) onHitParam[4] :-1;
            if (sightEffectId >= 0){
                UnitBindManager ubm = target.GetComponent<UnitBindManager>();
                if (ubm){
                    Debug.Log($"播放受击特效，部位{bpName}");
                    ubm.AddBindEffect(bpName, sightEffectId, "", false);
                }
            }

            if (audioId >= 0)
            {
                GameEntry.Sound.PlaySFX3D(audioId,bullet.transform.position);
            }

           
            if (target)
            {

                if (target.resource.hp >= target.property.hp)
                {
                    target.AddBuff(new AddBuffInfo(DesingerTables.Buff.data["ExtraHP"],bullet.caster,target.gameObject,1,1,true,true));
                    target.ModResource(new ChaResource(5));
                    GameEntry.FlyText.FlyText(target.transform.position,"HP+5");
                }
                else
                {
                    GameEntry.Combat.CreateDamage(
                        bulletState.caster, 
                        target.gameObject,
                        new Damage(Mathf.CeilToInt(-damageTimes * bulletState.propWhileCast.attack)),
                        critRate,
                        new DamageInfoTag[]{DamageInfoTag.directHeal, }
                    );
                }
              
               
            }
            
        }


       

       



        private static void ExplosionOnHit(BulletState bullet, ChaState target)
        {
            BulletState bulletState = bullet;
            if (!bulletState) return;
            object[] onHitParam = bulletState.model.onHitParams;
            float damageTimes = onHitParam.Length > 0 ? (float)onHitParam[0] : 1.00f;
            int explosionEffectId= onHitParam.Length > 1 ? (int)onHitParam[1] : 0;
            int explosionSoundId= onHitParam.Length > 2 ? (int)onHitParam[2] : 0;
            GameEntry.Combat.CreateAoe("ExplosionAoe",bullet.GetComponent<BulletState>().caster,bullet.transform.position,0.05f,Quaternion.identity, 11f,"",Array.Empty<object>(),new Dictionary<string, object>()
            {
                {"EffectIds","70001|70002|70003"},{"SoundIds","30005|30006|30007"},{"HitAlly","False"},{"DamageTimes",damageTimes.ToString()}
            });

            var effectGo = GameEntry.Entity.ShowEffectEntity(explosionEffectId, bullet.transform.position, Quaternion.identity);
            
          
            GameEntry.Sound.PlaySFX3D(explosionSoundId, bullet.transform.position);
            GameEntry.Combat.CreateAoe("KnockOffAoe",bullet.GetComponent<BulletState>().caster,bullet.transform.position,0.05f,Quaternion.identity, 11f,"",new object[0],new Dictionary<string, object>()
            {
                {"Force","12"},{"Degree","35"}
            });
            
        }
        private static void ExplosionOnRemoved(GameObject bullet)
        {
            BulletState bulletState = bullet.GetComponent<BulletState>();
            if (!bulletState) return;
            object[] onHitParam = bulletState.model.onHitParams;
            float damageTimes = onHitParam.Length > 0 ? (float)onHitParam[0] : 1.00f;
            int explosionEffectId= onHitParam.Length > 1 ? (int)onHitParam[1] : 0;
            int explosionSoundId= onHitParam.Length > 2 ? (int)onHitParam[2] : 0;
            GameEntry.Combat.CreateAoe("ExplosionAoe",bullet.GetComponent<BulletState>().caster,bullet.transform.position,0.05f,Quaternion.identity, 11f,"",Array.Empty<object>(),new Dictionary<string, object>()
            {
                {"EffectIds","70001|70002|70003"},{"SoundIds","30005|30006|30007"},{"HitAlly","False"},{"DamageTimes",damageTimes.ToString()}
            });

            var effectGo = GameEntry.Entity.ShowEffectEntity(explosionEffectId, bullet.transform.position, Quaternion.identity);
            
          
            GameEntry.Sound.PlaySFX3D(explosionSoundId, bullet.transform.position);
            GameEntry.Combat.CreateAoe("KnockOffAoe", bullet.GetComponent<BulletState>().caster,
                bullet.transform.position, 0.05f, Quaternion.identity, 11f, "", new object[0],
                new Dictionary<string, object>()
                {
                    { "Force", "12" }, { "Degree", "35" }
                });

        }

        ///<summary>
        ///onRemoved
        ///普通子结束，参数：
        ///[0]命中视觉特效Id
        ///</summary>
        private static void CommonBulletRemoved(GameObject bullet){
            BulletState bulletState = bullet.GetComponent<BulletState>();
            if (!bulletState) return;
            object[] onRemovedParams = bulletState.model.onRemovedParams;
            int sightEffect = onRemovedParams.Length > 0 ? (int)onRemovedParams[0] : -1;
            if (sightEffect >=0){
                GameEntry.Combat.CreateSightEffect(
                    sightEffect, 
                    bullet.transform.position, 
                    bullet.transform.rotation
                );      
            }

            int soundId = onRemovedParams.Length > 1 ? (int)onRemovedParams[1] : -1;
            if (soundId >=0){
                GameEntry.Sound.PlaySFX3D(
                    soundId, 
                    bullet.transform.position
                );      
            }
        }

        ///<summary>
        ///targetting
        ///选择最近的敌人作为目标
        ///</summary>
        private static GameObject GetNearestEnemy(GameObject bullet, GameObject[] targets){
            BulletState bs = bullet.GetComponent<BulletState>();
            string casterCamp = GameEntry.Const.CONST_Camp_Unknown;
            if (bs.caster){
                ChaState ccs = bs.caster.GetComponent<ChaState>();
                if (ccs) casterCamp = ccs.Camp;
            }

            GameObject bestTarget = null;
            float bestDis = float.MaxValue;
            for (int i = 0; i < targets.Length; i++){
                ChaState tcs = targets[i].GetComponent<ChaState>();
                if (!tcs || GameEntry.Combat.GetRelation(casterCamp,tcs.Camp)==RelationType.Friendly || tcs.dead == true) continue;
                float dis2 = (
                    Mathf.Pow(bullet.transform.position.x - targets[i].transform.position.x, 2) +
                    Mathf.Pow(bullet.transform.position.z - targets[i].transform.position.z, 2)
                );
                if (bestDis > dis2 || bestTarget == null){
                    bestTarget = targets[i];
                    bestDis = dis2;
                }
            }

            return bestTarget;
        }
        ///<summary>
        ///tween
        ///跟踪目标
        ///</summary>
        private static Vector3 FollowingTarget(float t, BulletState bullet, GameObject target){
            Vector3 res = Vector3.forward;
            if (target!=null){
                Vector3 tarDir = target.transform.position - bullet.transform.position;
                float flyingRad = (Mathf.Atan2(tarDir.x,  tarDir.z) * 180 / Mathf.PI - bullet.transform.eulerAngles.y) * Mathf.PI / 180;

                res.x = Mathf.Sin(flyingRad);
                res.z = Mathf.Cos(flyingRad);
                
            }
            return res;
        }

        ///<summary>
        ///targetting
        ///选择子弹的施法者作为跟踪的目标
        ///</summary>
        private static GameObject BulletCasterSelf(GameObject bullet, GameObject[] targets){
            BulletState bulletState = bullet.GetComponent<BulletState>();
            if (!bulletState) return null;
            return bulletState.caster;
        }

        
        ///<summary>
        ///onHit
        ///氪漏氪回力标命中效果，除了普通效果，就是命中自己的时候移除子弹，参数：
        ///[0]伤害倍数
        ///[1]基础暴击率
        ///[2]命中视觉特效
        ///[3]播放特效位于目标的绑点，默认Body
        ///</summary>
        private static void CloakBoomerangHit(BulletState bullet, ChaState target){
            BulletState bs = bullet;
            if (!bs) return;

            ChaState ccs = bs.caster.GetComponent<ChaState>();
            ChaState tcs = target.GetComponent<ChaState>();
            if (ccs != null && tcs != null && GameEntry.Combat.GetRelation(ccs.Camp,tcs.Camp)!=RelationType.Friendly){
                CommonBulletHit(bullet, target);
            }else{
                float backTime = bs.param.ContainsKey("backTime") ? (float)bs.param["backTime"] : 1.0f; //默认1秒 
                if (bs.timeElapsed > backTime && target.Equals(bs.caster)){
                    GameEntry.Combat.RemoveBullet(bullet);
                    if (ccs) ccs.PlaySightEffect("Body",70000);
                }
            }
        }

        ///<summary>
        ///Tween
        ///逐渐加速的子弹，bulletObj参数：
        ///["turningPoint"]float：在第几秒达到预设的速度（100%），并且逐渐减缓增速。
        ///</summary>
        private static Vector3 SlowlyFaster(float t, BulletState bullet, GameObject target){
            BulletState bs = bullet;
            if (!bs) return Vector3.forward;
            float tp = 5.0f; //默认5秒后达到100%速度
            if (bs.param.ContainsKey("turningPoint")) tp = (float)bs.param["turningPoint"];
            if (tp < 1.0f) tp = 1.0f;
            return Vector3.forward * (2 * t / (t + tp));
        }
        
        private static bool OnlyUseTweenOnFirstFrame(float t, BulletState bullet, GameObject target){
            BulletState bs = bullet;
            if (!bs) return false;

            bool hasSetInitialVelocity = bs.param.ContainsKey("hasSetInitialVelocity") && (bool)bs.param["hasSetInitialVelocity"];

            if (hasSetInitialVelocity)
            {
                return false;
            }
          
            bs.param.Add("hasSetInitialVelocity",true);
            return true;
            
        }
        
       

        

        

    
       
        ///<summary>
        ///onRemoved
        ///在子弹位置创建一个aoe，所以aoe的始作俑者肯定是caster了，位置也是子弹位置，填写什么都无效，角度也是子弹角度，参数：
        ///[0]AoeLauncher：aoe的发射器，caster在这里被重新赋值，position则作为增量加给现在的角色坐标
        ///[1]AoeLauncher：如果bullet移除时后duration>0或者是obstacled，则会创建这个，如果有这个的话
        ///</summary>
        private static void CreateAoEOnRemoved(GameObject bullet){
            BulletState bulletState = bullet.GetComponent<BulletState>();
            if (!bulletState) return;
            object[] onRemovedParams = bulletState.model.onRemovedParams;
            if (onRemovedParams.Length <= 0)    return;
            AoeLauncher al = (AoeLauncher)onRemovedParams[0];
            if (onRemovedParams.Length > 1 && (bulletState.duration > 0)) {
                al = (AoeLauncher)onRemovedParams[1];
            }
            al.caster = bulletState.caster;
            al.position = bullet.transform.position;
            al.rotation = bullet.transform.rotation;
            Debug.Log("to create aoe effect " + al.model.aoeEntityTypeId);
            GameEntry.Combat.CreateAoE(al);
        }

        ///<summary>
        ///onHit
        ///在子弹位置创建一个aoe，所以aoe的始作俑者肯定是caster了，位置也是子弹位置，填写什么都无效，角度也是子弹角度，参数：
        ///[0]AoeLauncher：aoe的发射器，caster在这里被重新赋值，position则作为增量加给现在的角色坐标
        ///</summary>
        private static void CreateAoEOnHit(BulletState bullet, ChaState target){
            BulletState bulletState = bullet;
            if (!bulletState) return;
            object[] onHitParams = bulletState.model.onHitParams;
            if (onHitParams.Length <= 0)    return;
            AoeLauncher al = (AoeLauncher)onHitParams[0];
            al.caster = bulletState.caster;
            al.position = bullet.transform.position;
            al.rotation = bullet.transform.rotation;
            GameEntry.Combat.CreateAoE(al);
        }

    }
}