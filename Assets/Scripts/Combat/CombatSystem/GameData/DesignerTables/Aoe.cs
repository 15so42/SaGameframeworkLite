using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingerTables
{
    
    ///<summary>
    ///AoeModel
    ///</summary>
    public class AoE{
        public static Dictionary<string, AoeModel> data = new Dictionary<string, AoeModel>(){
            {"BulletShield", new AoeModel(
                "BulletShield",60000 , new string[0], 0, true, 
                "", new object[0],  //create
                "", new object[0],  //remove
                "", new object[0],  //tick
                "", new object[0],  //chaEnter
                "", new object[0],  //chaLeave
                "BlockBullets", new object[]{false}, //bulletEnter
                "", new object[0]  //bulletLeave
            )},
            {"BlackHole", new AoeModel(
                "BlackHole", 60001, new string[0], 0.02f, true,
                "", new object[0],  //create
                "", new object[0],  //remove
                "BlackHole", new object[0],  //tick
                "", new object[0],  //chaEnter
                "", new object[0],  //chaLeave
                "", new object[0],  //bulletEnter
                "", new object[0]   //bulletLeave
            )},
            
            {"ExplosionAoe", new AoeModel(
                "ExplosionAoe", 0, new string[0], 0, true,
                "", new object[0],  //create
                "", new object[0],  //remove
                "", new object[0],  //tick
                "DoDamageToEnterCha", new object[]{new Damage(0,0,1),1f,true,false,true,"-1","-1","Body",false},  //参数为:伤害，倍率，对敌人有效，对友方有效，播放受击动画，特效，音效，特效部位，是否禁止移动
                "", new object[0],  //chaLeave
                "", new object[0],  //bulletEnter
                "", new object[0]   //bulletLeave
            )},
            {"HealAoe", new AoeModel(
                "ExplosionAoe", 0, new string[0], 0, true,
                "", new object[0],  //create
                "", new object[0],  //remove
                "", new object[0],  //tick
                "DoHealToEnterCha", new object[]{new Damage(0,0,0,1),1f,true,true,false,"-1","-1","Body",false},  //chaEnter
                "", new object[0],  //chaLeave
                "", new object[0],  //bulletEnter
                "", new object[0]   //bulletLeave
            )},
            {"AttackAoe", new AoeModel(
                "AttackAoe", 80000, new string[0], 0, true,
                "", new object[0],  //create
                "", new object[0],  //remove
                "", new object[0],  //tick
                "DoDamageToEnterCha", new object[]{new Damage(1),1f,true,false,true,"71000|71001","30000|30001|30002|30003|30004"},  //chaEnter
                "", new object[0],  //chaLeave
                "", new object[0],  //bulletEnter
                "", new object[0]   //bulletLeave
            )},
            
            {"KnockOffAoe", new AoeModel(
                "KnockOffAoe", 80000, new string[0], 0, true,
                "", new object[0],  //create
                "", new object[0],  //remove
                "", new object[0],  //tick
                "KnockOffEnterCha", new object[]{20,75f,false,true,true,false},  //击飞效果
                "", new object[0],  //chaLeave
                "", new object[0],  //bulletEnter
                "", new object[0]   //bulletLeave
            )},
            
           
        };
    }
}