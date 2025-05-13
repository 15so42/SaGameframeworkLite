using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class RigidbodyVelocityMoveLogic : UnitMoveLogic
{
    private Rigidbody rb;
    public override void Init(UnitMove unitMove)
    {
        base.Init(unitMove);

        rb = unitMove.gameObject.GetOrAddComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;//转换到update
    }

    public override Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    private Vector3 currentMoveVelocity;//控制器所需速度，和外力分开计算

    public override void Move( float deltaTime)
    {
      
        //rb.MovePosition(rb.position+targetVelocity*deltaTime);
        //return;//这个方法会导致抖动
        
        
        // var velocity = rb.velocity;
        // float maxSpeedChange = unitMove.speedParam * deltaTime;//加速度
        //
        // velocity.x =
        //     Mathf.MoveTowards(velocity.x, unitMove.targetVelocity.x, maxSpeedChange);
        // velocity.z =
        //     Mathf.MoveTowards(velocity.z, unitMove.targetVelocity.z, maxSpeedChange);
        // rb.velocity = velocity;


        rb.velocity = unitMove.targetVelocity;//覆盖所有
        //Debug.Log($"{unitMove.gameObject.name}设置rb.velocity为{rb.velocity}");
    }

    
   
}
