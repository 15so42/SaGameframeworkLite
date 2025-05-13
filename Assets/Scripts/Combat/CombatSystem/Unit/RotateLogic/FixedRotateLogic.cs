using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FixedRotateLogic : UnitRotateLogic
{
    
    public override void RotateTo(float deltaTime)
    {
        // 平滑旋转
        unitRotate.transform.rotation = Quaternion.RotateTowards(
            unitRotate.transform.rotation,
            unitRotate.targetRotation,
            unitRotate.rotateSpeedParams * deltaTime);
    }

   
}
