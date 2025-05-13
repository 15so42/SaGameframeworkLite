using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class EffectEntityUserData
{
    public float releaseTime = 0;
    public float scale = 1;

    public EffectEntityUserData(float releaseTime,float scale=1)
    {
        this.releaseTime = releaseTime;
        this.scale = scale;
    }
}

public class EffectEntity : Entity
{
    private EffectEntityDataRow effectEntityDataRow;
    private EffectEntityUserData effectEntityUserData;
    public override void Init(EntityDataRow entityDataRow,SoDataRow concreteDataRow, object userData)
    {
        base.Init(entityDataRow, concreteDataRow,userData);
        effectEntityDataRow=concreteDataRow as EffectEntityDataRow;

        if (userData != null)
        {
            this.effectEntityUserData = userData as EffectEntityUserData;
        }

        if (effectEntityUserData!=null)
        {
            transform.localScale = Vector3.one * effectEntityUserData.scale;
            if (effectEntityUserData.releaseTime > 0)
            {
                Timer.Register(effectEntityUserData.releaseTime, () =>
                {
                    Hide();
                });
            }
        }
        
    }
}
