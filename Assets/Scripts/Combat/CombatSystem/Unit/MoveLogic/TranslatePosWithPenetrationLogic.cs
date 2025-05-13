using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TranslatePosWithPenetrationLogic : UnitMoveLogic
{
    
    private CapsuleCollider capsuleCollider;
    public override Vector3 GetVelocity()
    {
        return unitMove.targetVelocity;
    }

    public override void Init(UnitMove unitMove)
    {
        base.Init(unitMove);
        capsuleCollider = base.unitMove.GetComponent<CapsuleCollider>();
    }

    private Vector3 predictedPosition;

    public override void Move(float deltaTime)
    {
        Vector3 movement = unitMove.targetVelocity * deltaTime;
        Vector3 newPosition = unitMove.transform.position + movement;

        // 尝试先移动角色
        unitMove.transform.position = newPosition;

        if(capsuleCollider==null)
            return;
        
        // 获取要忽略的层
        int dropItemLayer = LayerMask.NameToLayer("DropItem");

        // 创建排除DropItem层的掩码（所有层 异或 DropItem层）
        int layerMask = ~(1 << dropItemLayer);

        // 使用球体检测时传入掩码参数
        Collider[] overlaps = Physics.OverlapSphere(
            newPosition + Vector3.up * 0.5f, 
            0.5f, 
            layerMask);

        foreach (var other in overlaps)
        {
            if (other.gameObject == unitMove.gameObject) continue;

            // 如果有重叠，计算最小推出方向
            Vector3 direction;
            float distance;
            bool hasPenetration = Physics.ComputePenetration(
                capsuleCollider, unitMove.transform.position, unitMove.transform.rotation,
                other, other.transform.position, other.transform.rotation,
                out direction, out distance
            );

            if (hasPenetration)
            {
                // 立即将角色推出碰撞体
                direction.y = 0;
                unitMove.transform.position += direction * distance;
            }
        }
    }









    
}
