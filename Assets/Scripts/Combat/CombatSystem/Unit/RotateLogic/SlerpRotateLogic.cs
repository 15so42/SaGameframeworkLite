using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SlerpRotateLogic : UnitRotateLogic
{
    
    public override void RotateTo(float deltaTime)
    {
        unitRotate.transform.rotation = Quaternion.Slerp(
            unitRotate.transform.rotation,
            unitRotate.targetRotation,
            unitRotate.rotateSpeedParams * deltaTime);
    }
    
    
}