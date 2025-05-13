using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingerTables
{
    
    ///<summary>
    ///BulletModel
    ///</summary>
    public class Skill{
        public static Dictionary<string, SkillModel> data = new Dictionary<string, SkillModel>(){
            {"Fire", new SkillModel("Fire", new ChaResource(0, 0), ChaResource.Null, "Skill_Fire",Array.Empty<AddBuffInfo>())}, //即使没有子弹也可以用这个技能，但是因为有buff会让他自动转向另一个reload的timeline
           
            
        };
        
    }
}