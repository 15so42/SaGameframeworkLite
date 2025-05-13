using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BattleEntityDieStrategy : MonoBehaviour
{
   public virtual void Die(BattleEntity battleEntity)
   {
      battleEntity.Hide();
   }
}
