using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DesignerScripts;

using UnityEngine;
using UnityTimer;
using Buff = DesingerTables.Buff;

public class CombatUtility : MonoBehaviour
{
   public static void KnockOff(Rigidbody rb,  GameObject caster, GameObject carrier,Vector3 forwardDir, float upDegree, float force, bool ignoreX = false, bool ignoreZ = true, bool addBuff = true,Dictionary<string,object> buffParam=null,Dictionary<string,object> removeKnockedBuffParam=null)
   {
      // 计算向上的方向
      Vector3 upwardDir = Vector3.up;
      Vector3 liftDirection = Vector3.RotateTowards(forwardDir, upwardDir, Mathf.Deg2Rad * upDegree, 0f);

      if (ignoreX)
         liftDirection.x = 0;
      if (ignoreZ)
         liftDirection.z = 0;

      var inWater = new List<Buff>(); // Placeholder for actual inWater check
      rb.AddForce(liftDirection.normalized * (force * (inWater.Count > 0 ? 0.3f : 1f) ), ForceMode.Impulse);

      if (addBuff)
      {
         var cs = rb.GetComponent<ChaState>();
         if (cs == null)
            return;

         float duration = 10f;
         bool permanent = true;
         if (cs.tags.ToList().Contains("Fish"))
         {
            duration = 3;
            permanent = false;
         }
         

         cs.AddBuff(new AddBuffInfo(DesingerTables.Buff.data["KnockedOff"], caster, carrier, 1, duration, true, permanent,buffParam));
         
         //过0.3秒再落地检测，否则还没起飞就检测落地了
         Timer.Register(0.3f,()=>
         {
            if (cs != null)
            {
               cs.AddBuff(new AddBuffInfo(DesingerTables.Buff.data["RemoveKnockedOffBuffOnGround"], caster, carrier, 1, 10,
                  true, true,removeKnockedBuffParam));
               cs.AddBuff(new AddBuffInfo(DesingerTables.Buff.data["CollideAnimHandle"], caster, carrier, 1, 10, true,
                  true));
            }
           

         });
      }
   }

  
   
}
