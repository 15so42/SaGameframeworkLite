using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class FlyTextUiForm : UiForm
{

    public GameObject prefab;

    private ObjectPool<FlyTextUiItem> pool;

    public Transform root;
    
    public int textSize=64;
    public float worldJumpRadius = 1;
    public float worldJumpHeight = 1;
    public float jumpDuration = 2;
    public Ease ease = Ease.Linear;

    private void Awake()
    {
        pool = new ObjectPool<FlyTextUiItem>(() =>
        {
            var go = GameObject.Instantiate(prefab);
            go.gameObject.SetActive(false);
            return go.GetComponent<FlyTextUiItem>();
        },(x)=>{x.gameObject.SetActive(true);},
            (x)=>x.gameObject.SetActive(false));
    }

   
    public override void OnOpen()
    {
       base.OnOpen();
    }

    public void FlyText(Vector3 pos, string msg)
    {
        var flyTextUiItem = pool.Get();
        flyTextUiItem.transform.position = Camera.main.WorldToScreenPoint(pos);
        flyTextUiItem.transform.SetParent(root);
        flyTextUiItem.Init(this,pos,msg);
    }

    public void Release(FlyTextUiItem flyTextUiItem)
    {
        pool.Release(flyTextUiItem);
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public override bool HandleEscEvent()
    {
        return false;
    }
}
