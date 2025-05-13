using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RigidbodyPosMoveLogic : UnitMoveLogic
{
    private Rigidbody rb;
    public override Vector3 GetVelocity()
    {
        return unitMove.targetVelocity;
    }

    public override void Init(UnitMove unitMove)
    {
        base.Init(unitMove);
        rb = unitMove.GetComponent<Rigidbody>();
    }

    public override void Move(float deltaTime)
    {
        
        //rb.MovePosition(rb.position+ unitMove.targetVelocity*deltaTime);
        rb.position += unitMove.targetVelocity * deltaTime;
    }

    
}
