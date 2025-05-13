using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitMoveLogic
{
    protected UnitMove unitMove;
    public virtual void Init(UnitMove unitMove)
    {
        this.unitMove = unitMove;
    }

    public abstract Vector3 GetVelocity();
    
    public abstract void Move(float deltaTime);

    // /// <summary>
    // /// 用于清除加速度等变量
    // /// </summary>
    // public abstract void EnableMove();
    // public abstract void DisableMove();
}
