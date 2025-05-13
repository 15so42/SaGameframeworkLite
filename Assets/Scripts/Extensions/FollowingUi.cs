using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(100)]
public abstract class FollowingUi : MonoBehaviour
{
    protected Camera mainCam;
    protected Transform targetTransform;

    private RectTransform rectTransform;
    public int followingConfigId;
    protected  virtual void Awake()
    {
        mainCam=Camera.main;
        rectTransform=GetComponent<RectTransform>();
    }

    /// <summary>
    /// 根据阵营系统设置Ui颜色
    /// </summary>
    public abstract void UpdateColor(Color color);


    public void ForceSetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    public virtual void Init(int followingConfigId)
    {
        this.followingConfigId = followingConfigId;
        
    }

    public virtual void Bind(GameObject go, string bindPointKey)
    {
        var unitBindPointManager = go.GetOrAddComponent<UnitBindManager>();
        targetTransform = unitBindPointManager.GetBindPointByKey(bindPointKey).transform;
        mainCam = Camera.main;
    }

   

    private void LateUpdate()
    {
        if (targetTransform == null || mainCam == null) return;

        // 将世界坐标转换为屏幕坐标
        Vector3 screenPos = mainCam.WorldToScreenPoint(targetTransform.position);

        // 直接更新位置
        transform.position=screenPos;
        //transform.position=Vector3.Lerp(transform.position,screenPos,30*Time.deltaTime);
    }

    public void Release()
    {
        GameEntry.followingUi.Release(this,this.followingConfigId);
    }
    
    
}
