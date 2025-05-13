using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitRotateLogic
{
   public UnitRotate unitRotate;

   public virtual void Init(UnitRotate unitRotate)
   {
      this.unitRotate = unitRotate;
   }
   

   // 固定更新中调用，设置目标旋转并记录计数器
   public abstract void RotateTo(float deltaTime);




}
