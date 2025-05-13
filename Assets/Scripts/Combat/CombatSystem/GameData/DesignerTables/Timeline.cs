using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingerTables
{
    public class Timeline{
        //FireBullet参数======================================================
        //[0]BulletLauncher：子弹发射信息，其中caster和position是需要获得后改写的，填写null和Vector3.zero即可，旋转是局部旋转。
        //[1]string：角色身上绑点位置，默认Muzzle
        //[2]int:特效Id,默认-1
        //[3]int:音效Id,默认-1
        public static Dictionary<string, TimelineModel> data = new Dictionary<string, TimelineModel>(){
            //发射普通子弹
            { "Skill_Fire", new TimelineModel("Skill_Fire", new TimelineNode[]{
                //new TimelineNode(0.00f, "SetCasterControlState", new object[]{true, true, false}),//本项目中可以无限发射，其余项目可以设置控制效果
                //new TimelineNode(0.00f, "CasterPlayAnim", new object[]{UnitAnim.AnimOrderType.Trigger,"Fire" }),
                //new TimelineNode(0.10f, "PlaySightEffectOnCaster", new object[]{"Muzzle",2000,"",false}),
                new TimelineNode(0.0f, "FireBullet", new object[]{
                    new BulletLauncher(
                        Bullet.data["DefaultBullet"], null, Vector3.zero, Quaternion.identity, 10, 5.0f
                    ){hitCaster = false,useFireDegreeForever = true,bulletTweenCondition = DesignerScripts.Bullet.bulletTweenCondition["OnlyUseTweenOnFirstFrame"]}, "Muzzle"
                }),
                //new TimelineNode(0.0f, "SetCasterControlState", new object[]{true, true, true})
            }, 0.01f, TimelineGoTo.Null)},
            
            
        };


      
    }    
}