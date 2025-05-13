using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RigidbodyRotateLogic : UnitRotateLogic
{
    private Rigidbody rb;
   
    public override void Init(UnitRotate unitRotate)
    {
        base.Init(unitRotate);

        rb = unitRotate.gameObject.GetOrAddComponent<Rigidbody>();
    }


    public override void RotateTo(float deltaTime)
    {
        rb.MoveRotation( Quaternion.Slerp(rb.rotation,unitRotate.targetRotation,unitRotate.rotateSpeedParams*deltaTime));
    }

  
    
}
