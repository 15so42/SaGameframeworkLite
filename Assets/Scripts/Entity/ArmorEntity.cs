using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEntity : Entity
{
    public ArmorEntityDataRow armorEntityDataRow;
    public override void Init(EntityDataRow entityDataRow, SoDataRow concreteDataRow, object userData)
    {
        base.Init(entityDataRow, concreteDataRow, userData);
        this.armorEntityDataRow=concreteDataRow as ArmorEntityDataRow;
        
    }
}
