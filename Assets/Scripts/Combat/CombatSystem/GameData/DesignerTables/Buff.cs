using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingerTables
{
    ///<summary>
    ///buff的效果
    ///</summary>
    public class Buff{
        public static Dictionary<string, BuffModel> data = new Dictionary<string, BuffModel>(){
            
            { "Poison",new BuffModel("Poison","毒",new string[]{"Poison"},0,1,1.0f,
                    "",new object[0],
                    "", new object[0],  //occur
                    "", new object[0],  //remove
                    "DoPercentDamageToCarrier", new object[3]{"CurrentHealth",1f,0.5f},  //tick
                    "", new object[0],  //cast
                    "", new object[0],  //hit
                    "", new object[0],  //hurt
                    "", new object[0],  //kill
                    "", new object[0],  //dead
                    ChaControlState.origin, null  
            )},
          
            { "Stun", new BuffModel("Stun", "眩晕", new string[]{"Control"}, 0, 1, 0f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.stun, null  
            )},
           
            
            { "ShowBattleUnitOnBeKilled", new BuffModel("ShowBattleUnitOnBeKilled", "死亡死生成新单位", new string[]{"Passive"}, 0, 1, 0f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "ShowBattleUnitOnBeKilled", new object[]{20018},  //dead
                ChaControlState.origin, null  
            )},
            
           
            { "DamageAttackerOnHurt", new BuffModel("DamageAttackerOnHurt", "反伤", new string[]{"Passive"}, 0, 1, 0f,
                "",new object[0],
                "", new object[]{},  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "DamageAttackerOnHurt", new object[]{},  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  
            )},
            { "BeAttacked", new BuffModel("BeAttacked", "受伤硬直", new string[]{"Passive"}, 0, 1, 0f,
                "",new object[0],
                "", new object[]{},  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[]{},  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.disableMove, null  
            )},
            
            
            { "KnockOffOtherOnCollide", new BuffModel("KnockOffOtherOnCollide", "击飞碰到的角色", new string[]{"Passive"}, 0, 1, 0f,
                "KnockOffOtherOnCollide",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            
            { "ExplosionOnCollide", new BuffModel("ExplosionOnCollide", "碰撞时爆炸", new string[]{"Passive"}, 0, 1, 0f,
                "ExplosionOnCollide",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            { "ExplosionOnBeKilled", new BuffModel("ExplosionOnBeKilled", "死亡时爆炸", new string[]{"Passive"}, 0, 1, 0f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "ExplosionOnBeKilled", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            
            
            
            //特定项目内容
            { "AutoExplode", new BuffModel("AutoExplode", "定时爆炸", new string[]{"Passive","BuildingAbility"}, 0, 1, 3f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "AutoExplode", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            
            { "AutoHealExplode", new BuffModel("AutoHealExplode", "定时治疗", new string[]{"Passive","BuildingAbility"}, 0, 1, 3f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "AutoHealExplode", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            
            { "AutoFireBullet", new BuffModel("AutoFireBullet", "自动射击", new string[]{"Passive","BuildingAbility"}, 0, 1, 3f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "AutoFireBullet", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            { "AutoSpawnCoin", new BuffModel("AutoSpawnCoin", "自动生成金币", new string[]{"Passive","BuildingAbility"}, 0, 1, 5f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "AutoSpawnCoin", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, null  //桶子也是被昏迷的
            )},
            
            //玩家强化
            { "PlayerAddFireBulletCount", new BuffModel("PlayerAddFireBulletCount", "增加发射子弹数量", new string[]{"Passive"}, 0, 999, 1f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin,null  //桶子也是被昏迷的
            )},
            
            { "PlayerSubFireInterval", new BuffModel("PlayerSubFireInterval", "减少10%子弹发射间隔", new string[]{"Passive"}, 0, 999, 1f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin,null  //桶子也是被昏迷的
            )},
            
            { "PlayerAddBulletLife", new BuffModel("PlayerAddBulletLife", "增加子弹弹射寿命", new string[]{"Passive"}, 0, 999, 1f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin,null  //桶子也是被昏迷的
            )},
            
            //单位血量
            { "ExtraHP", new BuffModel("ExtraHP", "额外生命", new string[]{"Passive"}, 0, 999, 5f,
                "",new object[0],
                "", new object[0],  //occur
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //cast
                "", new object[0],  //hit
                "", new object[0],  //hurt
                "", new object[0],  //kill
                "", new object[0],  //dead
                ChaControlState.origin, new ChaProperty[2]{new ChaProperty(0,5){actionSpeed = 0},new ChaProperty()}  //桶子也是被昏迷的
            )},
            
           
            
        };
    }
}