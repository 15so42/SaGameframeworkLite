using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntityPlaceholder : EntityPlaceholder
{
    
    public override void ShowEntity()
    {
        GameEntry.Entity.ShowBattleEntity(id, transform.position, transform.rotation);
    }
}
