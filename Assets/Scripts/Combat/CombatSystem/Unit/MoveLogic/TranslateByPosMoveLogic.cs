using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateByPosMoveLogic : UnitMoveLogic
{
   

    public override Vector3 GetVelocity()
    {
        return unitMove.targetVelocity;
    }

    public override void Move(float deltaTime)
    {
        unitMove.transform.position += GetVelocity() * deltaTime;
    }
}
