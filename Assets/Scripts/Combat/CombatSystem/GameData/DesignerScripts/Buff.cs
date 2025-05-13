using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

using UnityEngine;
using UnityEngine.XR;
using UnityTimer;
using Random = UnityEngine.Random;

namespace DesignerScripts
{
    ///<summary>
    ///buff的效果
    ///</summary>
    public class Buff{
        public static Dictionary<string, BuffOnCollide> onCollideFunc = new Dictionary<string, BuffOnCollide>(){
            {"ExplosionOnCollide", ExplosionOnCollide},//碰撞时爆炸
            {"KnockOffOtherOnCollide", KnockOffOtherOnCollide},//碰撞时击飞
            {"DamageOtherOnCollide", DamageOtherOnCollide},//碰撞时伤害
        };
        //在BuffObj被创建、或者已经存在的BuffObj层数发生变化（但结果并不小于等于0）的时候，会出触发的脚本：
        public static Dictionary<string, BuffOnOccur> onOccurFunc = new Dictionary<string, BuffOnOccur>(){
          
        };
        public static Dictionary<string, BuffOnRemoved> onRemovedFunc = new Dictionary<string, BuffOnRemoved>(){
            
           
        };
        public static Dictionary<string, BuffOnTick> onTickFunc = new Dictionary<string, BuffOnTick>(){
            {"DoPercentDamageToCarrier", DoPercentDamageToCarrier},
            {"AutoExplode",AutoExplode},
            {"AutoHealExplode",AutoHealExplode},
            {"AutoFireBullet",AutoFireBullet},
            {"AutoSpawnCoin",AutoSpawnCoin},
        };
        public static Dictionary<string, BuffOnCast> onCastFunc = new Dictionary<string, BuffOnCast>(){
            
        };
        public static Dictionary<string, BuffOnHit> onHitFunc = new Dictionary<string, BuffOnHit>(){
           //击中的Buff
        };
        public static Dictionary<string, BuffOnBeHurt> beHurtFunc = new Dictionary<string, BuffOnBeHurt>(){
            {"OnlyTakeOneDirectDamage", OnlyTakeOneDirectDamage},//最多只能受到一滴伤害
            {"DamageAttackerOnHurt", DamageAttackerOnHurt},//受到攻击时对攻击者造成伤害
            
        };
        public static Dictionary<string, BuffOnKill> onKillFunc = new Dictionary<string, BuffOnKill>(){
            //击杀别人  
        };
        public static Dictionary<string, BuffOnBeKilled> beKilledFunc = new Dictionary<string, BuffOnBeKilled>(){
            {"ExplosionOnBeKilled", ExplosionOnBeKilled},
            {"ShowBattleUnitOnBeKilled", ShowBattleUnitOnBeKilled},
          
        };
        
     
        
        public static void DamageAttackerOnHurt(BuffObj buffObj, ref DamageInfo damageInfo, GameObject attacker)
        {
            //应该添加更多参数，之后用到再说
            var targetCha = buffObj.carrier.GetComponentInParent<ChaState>();
            if (targetCha && attacker!=null)
            {
                GameEntry.Combat.CreateDirectDamage(buffObj.carrier,attacker,new Damage(0,0,0,1));

            }
        }
        
      

        

        ///<summary>
        ///beHurt
        ///buff的Carrier只能受到1点直接伤害，免疫其他一切，桶子就是这样的
        ///</summary>
        private static void OnlyTakeOneDirectDamage(BuffObj buff, ref DamageInfo damageInfo, GameObject attacker){
            bool isDirectDamage = false;
            for (int i = 0; i < damageInfo.tags.Length; i++){
                if (damageInfo.tags[i] == DamageInfoTag.directDamage){
                    isDirectDamage = true;
                    break;
                }
            }
            if (isDirectDamage == true && damageInfo.DamageValue(false) > 0){
                int finalDV = 1;
                if (attacker != null){
                    ChaState cs = attacker.GetComponent<ChaState>();
                    //来自另外一个桶子（不包含自己）的伤害为9999，其他的都是1
                    if (cs != null && cs.HasTag("Barrel") == true && attacker.Equals(buff.carrier) == false){
                        finalDV = 9999;
                    }
                }
                damageInfo.damage = new Damage(0, finalDV);
            }else{
                damageInfo.damage = new Damage(0);
            }
        }
      
      

     


      
        
        

      
        /// <summary>
        /// 碰撞到别人时击飞别人
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="collide"></param>
        private static void KnockOffOtherOnCollide(BuffObj buff, GameObject collide)
        {

            var targetCha = collide.GetComponentInParent<ChaState>();
         
            if (targetCha)
            {

                    var scale = 6f;
                    
                    buff.buffParam.TryGetValue("Force", out var forceParam);
                    buff.buffParam.TryGetValue("Degree", out var degreeParam);
                    buff.buffParam.TryGetValue("Scale", out var scaleParam);
                 
                    if (scaleParam != null)
                    {
                        scale = float.Parse((string)scaleParam);
                    }
                    buff.buffParam.TryGetValue("DamageTimes", out var damageTimesParam);
            
                    Dictionary<string, object> knockOffAoeParam = new Dictionary<string, object>();
                    knockOffAoeParam.Add("Force",forceParam);
                    knockOffAoeParam.Add("Degree",degreeParam);

                    Dictionary<string, object> attackAoeParam = new Dictionary<string, object>();
                    attackAoeParam.Add("DamageTimes",damageTimesParam);
                  
                    
                    if (targetCha) 
                    {
                        GameEntry.Combat.CreateAoE(new AoeLauncher(DesingerTables.AoE.data["KnockOffAoe"],buff.carrier,buff.carrier.transform.position,0.05f,Quaternion.identity,scale,null,null,knockOffAoeParam));
                        //伤害Aoe
                        GameEntry.Combat.CreateAoE(new AoeLauncher(DesingerTables.AoE.data["AttackAoe"],buff.carrier,buff.carrier.transform.position,0.05f,Quaternion.identity,scale,null,null,attackAoeParam));

                    }

            }
        }
        
        
        private static void DamageOtherOnCollide(BuffObj buff, GameObject collide)
        {

            //var carrierCha = buff.carrier.GetComponent<ChaState>();
            var targetCha = collide.GetComponentInParent<ChaState>();
            if (targetCha)
            {
                buff.buffParam.TryGetValue("MeleeDamage", out var meleeDamageParam);
                buff.buffParam.TryGetValue("DamageTimes", out var damageTimesParam);
                buff.buffParam.TryGetValue("ToFoe", out var toFoeParam);
                buff.buffParam.TryGetValue("ToAlly", out var toAllyParam);
                buff.buffParam.TryGetValue("Scale", out var scaleParam);

                if (scaleParam == null)
                    scaleParam = 5;
            
                Dictionary<string, object> aoeParam = new Dictionary<string, object>();
                aoeParam.Add("MeleeDamage",meleeDamageParam);
                aoeParam.Add("DamageTimes",damageTimesParam);
                aoeParam.Add("ToFoe",toFoeParam);
                aoeParam.Add("ToAlly",toAllyParam);
                
                if (targetCha) 
                {
                    //伤害Aoe
                    GameEntry.Combat.CreateAoE(new AoeLauncher(DesingerTables.AoE.data["AttackAoe"],buff.carrier,buff.carrier.transform.position,0.05f,Quaternion.identity,float.Parse((string)scaleParam),null,null,aoeParam));

                }

            }
        }
        
            
        /// <summary>
        /// 碰撞到别人时爆炸
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="collide"></param>
        private static void ExplosionOnCollide(BuffObj buff, GameObject collide)
        {
           
            buff.buffParam.TryGetValue("Scale", out var scale);
            float targetScale=scale==null?12:(float)scale;
            
            buff.buffParam.TryGetValue("ExplosionEffectId", out var explosionEffectId);
            int targetExplosionEffectId=explosionEffectId==null?0:(int)explosionEffectId;
            
            
            GameEntry.Combat.CreateAoE(new AoeLauncher(DesingerTables.AoE.data["ExplosionAoe"],buff.carrier,buff.carrier.transform.position,0.1f,Quaternion.identity,targetScale));
            GameEntry.Combat.CreateAoE(new AoeLauncher(DesingerTables.AoE.data["KnockOffAoe"],buff.caster,buff.carrier.transform.position,0.1f,Quaternion.identity,targetScale));
            GameEntry.Entity.ShowEffectEntity(targetExplosionEffectId,
                buff.carrier.transform.position, Quaternion.identity);

        }
   

       
       

      

        private static void DoPercentDamageToCarrier(BuffObj buff)
        {
            var cha = buff.carrier.GetComponent<ChaState>();
            if(cha==null)
                return;
            var onTickParams = buff.model.onTickParams;
            var damageType = onTickParams.Length > 0 ? (string)onTickParams[0] : "MaxHealth";
            var basePercent = onTickParams.Length > 1 ? (float)onTickParams[1] : 1;
            var stackPercent = onTickParams.Length > 2 ? (float)onTickParams[2] : 0;

            float damageValue = 0;
            if (damageType.ToLower().Contains("max"))
            {
                damageValue = cha.property.hp* (basePercent+stackPercent*buff.stack)*0.01f ;
            }
            else if(damageType.ToLower().Contains("current"))
            {
                damageValue = cha.resource.hp* (basePercent+stackPercent*buff.stack)*0.01f ;

            }
            GameEntry.Combat.CreateDamage(buff.caster, buff.carrier, new Damage(0,0,0,Mathf.CeilToInt(damageValue)), 0, new DamageInfoTag[]{DamageInfoTag.periodDamage});

        }
        
        
        private static void AutoExplode(BuffObj buff)
        {
            var cha = buff.carrier.GetComponent<ChaState>();
            if(cha==null)
                return;
            var onTickParams = buff.model.onTickParams;

           
            buff.buffParam.TryGetValue("Scale", out var scaleObject);
            
            var scale = scaleObject==null?15:(int)scaleObject;
            
            //aoeParam.Add("DamageTimes",damageTimes);
            
            GameEntry.Combat.CreateAoE(new AoeLauncher(
                DesingerTables.AoE.data["ExplosionAoe" ?? string.Empty], buff.carrier,
                buff.carrier.transform.position, 0.1f, Quaternion.identity, scale, null, null));

            var effect=GameEntry.Entity.ShowEffectEntity(2, cha.transform.position, Quaternion.identity,
                new EffectEntityUserData(3,scale));
            //effect.transform.localScale=Vector3.one*2.6f;

            GameEntry.Sound.PlaySFX3D(600, cha.transform.position);
            
           
        }
        
        
        private static void AutoFireBullet(BuffObj buff)
        {
            var cha = buff.carrier.GetComponent<ChaState>();
            if(cha==null)
                return;
            var onTickParams = buff.model.onTickParams;
            
            
            var timelineModel = new TimelineModel("FireBullet", new TimelineNode[]
            {
                new TimelineNode(0.0f, "FireBullet", new object[]
                {
                    new BulletLauncher(
                        DesingerTables.Bullet.data["FortBullet"]
                        , null, Vector3.zero, Quaternion.identity, 10, 5.0f
                    )
                    {
                        hitCaster = false, useFireDegreeForever = true,
                        bulletTweenCondition = DesignerScripts.Bullet.bulletTweenCondition["OnlyUseTweenOnFirstFrame"]
                    },
                    "Muzzle"
                }),
            }, 0.01f, TimelineGoTo.Null);
            
            GameEntry.Combat.CreateTimeline(timelineModel,buff.carrier,null);
            
        }
        
        private static void AutoHealExplode(BuffObj buff)
        {
            var cha = buff.carrier.GetComponent<ChaState>();
            if(cha==null)
                return;
            var onTickParams = buff.model.onTickParams;
            
            buff.buffParam.TryGetValue("Scale", out var scaleObject);
            var scale = scaleObject==null?15:(int)scaleObject;
            //aoeParam.Add("DamageTimes",damageTimes);
            
            GameEntry.Combat.CreateAoE(new AoeLauncher(
                DesingerTables.AoE.data["HealAoe" ?? string.Empty], buff.carrier,
                buff.carrier.transform.position, 0.1f, Quaternion.identity, scale, null, null));

            var effect=GameEntry.Entity.ShowEffectEntity(3, cha.transform.position, Quaternion.identity,
                new EffectEntityUserData(3,scale));
          

            //GameEntry.Sound.PlaySFX3D(600, cha.transform.position);

        }
        
        private static void AutoSpawnCoin(BuffObj buff)
        {
            var cha = buff.carrier.GetComponent<ChaState>();
            if(cha==null)
                return;
            var onTickParams = buff.model.onTickParams;
            
            var randomSphere = Random.insideUnitSphere; 
            randomSphere.y = Mathf.Abs(randomSphere.y); // 确保 Y 方向不为负数

            var spawnPosition = buff.carrier.transform.position + Vector3.up * 2f + randomSphere;
            var coin = GameEntry.Entity.ShowModelEntity(3502, spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));

            // 获取 Rigidbody
            Rigidbody rb = coin.GetComponent<Rigidbody>();
            if (rb != null)
            {
             
                rb.AddExplosionForce(14, buff.carrier.transform.position - Vector3.up, 5, 1.0f, ForceMode.Impulse);
            }


        }
        

        private static void ShowBattleUnitOnBeKilled(BuffObj buff, DamageInfo damageInfo, GameObject attacker)
        {
            var onBeKilledParams = buff.model.onBeKilledParams;
            var typeId = onBeKilledParams.Length > 0 ? (int)onBeKilledParams[0] : 0;
            if (buff.buffParam.TryGetValue("TypeId", out var newTypeId))
            {
                if (newTypeId != null)
                {
                    typeId = int.Parse((string)newTypeId);
                }
            }
            if (typeId > 0)
            {
                var pos = buff.carrier.transform.position ;
                var rotation =buff.carrier.transform.rotation;
                var camp = buff.carrier.GetComponent<ChaState>().Camp;

                
                UnityTimer.Timer.Register(0.1f, () =>
                {
                    
                    var battleEntity = GameEntry.Entity.ShowBattleEntity(typeId, pos, rotation);
                    battleEntity.chaState.Camp=camp;

                });

            }
        }

        private static void ExplosionOnBeKilled(BuffObj buff, DamageInfo damageInfo, GameObject attacker)
        {
            
            buff.buffParam.TryGetValue("ExplosionAoeId", out var explosionAoeId);
            buff.buffParam.TryGetValue("PlayExplosionFx", out var playExplosionFx);
            buff.buffParam.TryGetValue("ExplosionEffectId", out var explosionEffectId);
            buff.buffParam.TryGetValue("ExplosionSoundId", out var explosionSoundId);
            //创建Aoe
            buff.buffParam.TryGetValue("ExplosionDamage", out var explosionDamage);
            buff.buffParam.TryGetValue("DamageTimes", out var damageTimes);
            buff.buffParam.TryGetValue("ToFoe", out var toFoe);
            buff.buffParam.TryGetValue("ToAlly", out var toAlly);
            buff.buffParam.TryGetValue("HurtAction", out var hurtAction);
            buff.buffParam.TryGetValue("EffectIds", out var effectIds);
            buff.buffParam.TryGetValue("SoundIds", out var soundIds);
            buff.buffParam.TryGetValue("OnlyDamage", out var onlyDamageParam);//只造成伤害的话就不会使用击飞aoe
            buff.buffParam.TryGetValue("Force", out var forceParam);//击飞Aoe的力度
            
            
            if (explosionAoeId == null)
                explosionAoeId = "ExplosionAoe";
            
            if (playExplosionFx == null)//可能不播放爆炸特效而是播放新的攻击Aoe特效
                playExplosionFx = "True";


            //这里的aoeParam是用来产生爆炸时的伤害Aoe和击飞Aoe的
            Dictionary<string, object> aoeParam = new Dictionary<string, object>();
            
            
            if (forceParam != null)
            {
                aoeParam.Add("Force",forceParam);
            }
            
            //只有参数有变更才加到aoe的参数中
            if (explosionDamage != null)
            {
                aoeParam.Add("ExplosionDamage",explosionDamage);
            }
            if (damageTimes != null)
            {
                aoeParam.Add("DamageTimes",damageTimes);
            }
            if (toFoe != null)
            {
                aoeParam.Add("ToFoe",toFoe);
            }
            if (toAlly != null)
            {
                aoeParam.Add("ToAlly",toAlly);
            }
            if (hurtAction != null)
            {
                aoeParam.Add("HurtAction",hurtAction);
            }
            if (effectIds != null)
            {
                aoeParam.Add("EffectIds",effectIds);
            }
            if (soundIds != null)
            {
                aoeParam.Add("SoundIds",soundIds);
            }

            //传递阵营用于死亡后阵营判定,因为是死亡后出发，aoe触发的时候carrier已经死了
            aoeParam.Add("Camp",buff.carrier.GetComponent<ChaState>().Camp);
            
           
           

            //延时，避免同帧把自己炸死多次，然后又触发ExplosionOnBekilled导致死循环
           Timer.Register(0.05f,() =>
            {
                if (onlyDamageParam != null && bool.Parse((string)onlyDamageParam) == true)
                {
                    GameEntry.Combat.CreateAoE(new AoeLauncher(
                        DesingerTables.AoE.data[(string)explosionAoeId ?? string.Empty], buff.carrier,
                        buff.carrier.transform.position, 0.1f, Quaternion.identity, 12f, null, null, aoeParam));

                }
                else
                {
                    GameEntry.Combat.CreateAoE(new AoeLauncher(DesingerTables.AoE.data["KnockOffAoe"], buff.carrier,
                        buff.carrier.transform.position, 0.1f, Quaternion.identity, 12f));
                    GameEntry.Combat.CreateAoE(new AoeLauncher(
                        DesingerTables.AoE.data[(string)explosionAoeId ?? string.Empty], buff.carrier,
                        buff.carrier.transform.position, 0.1f, Quaternion.identity, 12f, null, null, aoeParam));

                }
            });
            
           
            if (bool.Parse((string)playExplosionFx))
            {
                GameEntry.Entity.ShowEffectEntity((int)explosionEffectId, buff.carrier.transform.position,
                    Quaternion.identity);
                
            }
            
            GameEntry.Sound.PlaySFX3D((int)explosionSoundId,buff.carrier.transform.position);
            
          
        }
    }
}