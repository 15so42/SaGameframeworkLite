using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FollowingUiComponent : IGameComponent
{
    private Dictionary<int, ObjectPool<FollowingUi>> pools = new Dictionary<int, ObjectPool<FollowingUi>>();


    private FollowingUiForm followingUiForm;

    public FollowingUiForm FollowingUiForm
    {
        get
        {
            if (followingUiForm == null)
            {
                followingUiForm = NewFollowingUiForm();
            }

            return followingUiForm;
        }
    }
    private void OnEnable()
    {
        TypeEventSystem.Register<EntityShowEvent>(OnEntityShow);
    }


    private void Start()
    {
        NewFollowingUiForm();
    }


    void OnEntityShow(EntityShowEvent entityShow)
    {

        var requireFollowingUi = entityShow.entity.GetComponent<RequireFollowingUi>();
        if(!requireFollowingUi)
            return;
        
        
        var id = entityShow.entity.GetComponent<RequireFollowingUi>().targetFollowingUiId;
        ShowFollowingUi(entityShow.entity.gameObject,requireFollowingUi,id);
    }

    public FollowingUi ShowFollowingUi(GameObject target,RequireFollowingUi requireFollowingUi,int id)
    {
        var dataRow = GameEntry.SoDataTable.GetSoDataRow<FollowingUiDataRow>(id);
        if(dataRow==null)
            return null;

        FollowingUi followingUi = null;
        
        //先从对象池里拿
        if (!pools.ContainsKey(id))
        {
            //添加一个pool
            var pool = new ObjectPool<FollowingUi>(() =>
            {
                var tmpGo = GameObject.Instantiate(dataRow.pfb).GetComponent<FollowingUi>();
                return tmpGo;
                
            });
            
            pools.Add(id,pool);
        }
       
        followingUi = pools[id].Get();
        
        if(followingUiForm==null||followingUiForm.closed)//因为切换场景的时候所有Uiform会被关闭
            NewFollowingUiForm();
        followingUi.transform.SetParent(followingUiForm.transform);
        
        followingUi.gameObject.SetActive(true);
        
        followingUi.Init(id);
        requireFollowingUi.SetFollowingUi(followingUi);
        followingUi.Bind(target,dataRow.bindPointKey);

        return followingUi;

    }


    FollowingUiForm NewFollowingUiForm()
    {
        followingUiForm=GameEntry.Ui.ShowUiForm(2) as FollowingUiForm;
         return followingUiForm;
    }

    private void OnDisable()
    {
        TypeEventSystem.UnRegister<EntityShowEvent>(OnEntityShow);
    }


    public void Release(FollowingUi followingUi,int id)
    {
        if(followingUi.gameObject.activeSelf==false)
            return;
        pools[id].Release(followingUi);
        followingUi.gameObject.SetActive(false);
    }
    
   
}
