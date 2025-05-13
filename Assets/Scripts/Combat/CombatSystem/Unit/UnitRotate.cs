using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;


///<summary>
///单位旋转控件，如果一个单位要通过游戏逻辑来进行旋转，就应该用它，不论是角色还是aoe还是bullet什么的
///</summary>
public class UnitRotate : MonoBehaviour
{
    ///<summary>
    ///单位当前是否可以旋转角度
    ///</summary>
    [ReadOnly]
    public bool canRotate = true;


    public RotateMethod unitRotateMethod;
    public UnitRotateLogic unitRotateLogic;

    public Quaternion targetRotation;
    
    /// <summary>
    /// rotateLogic旋转的速度相关属性，比如slerp的速度，固定旋转速度等
    /// </summary>
    public float rotateSpeedParams=5f;


    public bool OverrideNavMeshAgent
    {
        set
        {
            var navmeshAgent = GetComponent<NavMeshAgent>();
            if (navmeshAgent)
            {
                navmeshAgent.updateRotation = value;
                navmeshAgent.updateUpAxis = value;
            }
        }
    }


  
    
    public void Init(RotateMethod rotateMethod, float speedParam)
    {
        this.rotateSpeedParams = speedParam;
        this.unitRotateMethod = rotateMethod;
        
    }

    private void Start()
    {
        switch (unitRotateMethod)
        {
            case RotateMethod.Slerp:
                unitRotateLogic = new SlerpRotateLogic();
                break;
            case RotateMethod.Fixed:
                unitRotateLogic = new FixedRotateLogic();
                break;
            case RotateMethod.RigidbodySlerp:
                unitRotateLogic = new RigidbodyRotateLogic();
                break;
           
        }
        
        unitRotateLogic.Init(this);
    }

 
    ///<summary>
    ///旋转到指定角度
    ///<param name="degree">需要旋转到的角度</param>
    ///</summary>
    public void RotateTo(Quaternion rotation,float deltaTime)
    {

        targetRotation = rotation;
        if(canRotate)
            unitRotateLogic.RotateTo(deltaTime);
    }

    

    ///<summary>
    ///禁止单位可以旋转的能力，这会终止当前正在进行的旋转
    ///终止当前的旋转看起来是一个side-effect，但是依照游戏规则设计来说，他只是“配套功能”所以严格的说并不是side-effect
    ///</summary>
    public void DisableRotate(){
        canRotate = false;
        
    }

    ///<summary>
    ///开启单位可以旋转的能力
    ///</summary>
    public void EnableRotate(){
        canRotate = true;
        
    }
}
