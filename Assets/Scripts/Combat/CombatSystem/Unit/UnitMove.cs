using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

///<summary>
///单位移动控件，所有需要移动的单位都应该添加这个来控制它的移动，不论是角色，aoe还是子弹，但是地形不能用这个，因为地形是terrain不是unit
///这里负责的是每一帧往一个方向移动，至于往什么方向移动，这应该是其他控件的事情，比如角色是由操作来决定的，子弹则是轨迹决定的
///</summary>
public class UnitMove : MonoBehaviour
{
    //是否有权移动
    public bool canMove = true;
    
    [ReadOnly]
    public MoveMethod unitMoveMethod = MoveMethod.RigidbodyVelocity;
    private UnitMoveLogic unitMoveLogic;


    public Vector3 targetVelocity;

    public virtual Vector3 GetVelocity()
    {
        return unitMoveLogic.GetVelocity();
    }
  


    /// <summary>
    /// moveLogic使用的速度变量，比如加速度，Lerp速度，缓动时间等
    /// </summary>
    public float speedParam;
    
    public void Init(MoveMethod moveMethod, float speedParam)
    {
        this.speedParam = speedParam;
        this.unitMoveMethod = moveMethod;
        
    }

    private void Start()
    {
        switch (unitMoveMethod)
        {
            case MoveMethod.RigidbodyVelocity:
                unitMoveLogic = new RigidbodyVelocityMoveLogic();
                break;
            case MoveMethod.TranslatePosWithPenetration:
                unitMoveLogic = new TranslatePosWithPenetrationLogic();
                break;
            case MoveMethod.RigidbodyPos:
                unitMoveLogic = new RigidbodyPosMoveLogic();
                break;
            case MoveMethod.TranslatePos:
                unitMoveLogic = new TranslateByPosMoveLogic();
                break;
            
           
        }
        
        unitMoveLogic.Init(this);
    }


   
    
    ///<summary>
    ///每秒移动传入的Vector3，也就是说要传入的是单位方向*速度
    ///<param name="moveForce">一秒的移动向量，包括长度</param>
    ///</summary>
    public void MoveBy(Vector3 moveForce,float deltaTime)
    {
        targetVelocity = moveForce;
        if(canMove) 
            unitMoveLogic.Move(deltaTime);
        
    }

 
    ///<summary>
    ///禁止角色可以移动能力
    ///</summary>
    public void DisableMove()
    {
        canMove = false;
    }

    ///<summary>
    ///开启角色可以移动的能力
    ///</summary>
    public void EnableMove(){
        canMove = true;
    }
}   


