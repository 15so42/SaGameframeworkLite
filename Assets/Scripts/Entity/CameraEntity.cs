using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraEntity : Entity
{
   private Entity targetEntity;

   private CinemachineVirtualCamera cinemachineVirtualCamera;

   public override void Init(EntityDataRow entityDataRow,SoDataRow concreteDataRow, object userData)
   {
      base.Init(entityDataRow,concreteDataRow, userData);
      cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
   }

   public void SetTarget(CameraFollowAndLookConfig cameraFollowAndLookConfig)
   {
      Transform lookAtTarget = cameraFollowAndLookConfig.GetLookAtTransform();
      Transform followTarget = cameraFollowAndLookConfig.GetFollowTransform();

      // 设置Follow目标
      cinemachineVirtualCamera.Follow = followTarget;

      // 设置LookAt目标并配置Composer
      if (lookAtTarget != null)
      {
         cinemachineVirtualCamera.LookAt = lookAtTarget;

         // 关键步骤：添加或获取Composer组件
         var composer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
         if (composer == null)
         {
            // 添加Composer组件（自动成为Aim模块）
            composer = cinemachineVirtualCamera.AddCinemachineComponent<CinemachineComposer>();
         }

         // 配置Composer参数（示例值）
         composer.m_ScreenX = 0.5f;   // 屏幕中心X
         composer.m_ScreenY = 0.5f;   // 屏幕中心Y
         composer.m_DeadZoneWidth = 0.1f;
         composer.m_DeadZoneHeight = 0.1f;
         composer.m_SoftZoneWidth = 0.8f;
         composer.m_SoftZoneHeight = 0.8f;
         
      }
   }
   public void SetTarget(Entity entity) 
   {
      var battleEntity = entity as BattleEntity;
      SetTarget(battleEntity.cameraFollowAndLookConfig);
      
   }
}
