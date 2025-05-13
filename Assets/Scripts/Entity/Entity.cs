using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Timer = UnityTimer.Timer;

public abstract class Entity : MonoBehaviour//Entity和UiForm不同，每个UIForm使用单独的UiForm子类，Entity则不同，Entity是一类Entity使用一种Entity子类
{
    public EntityDataRow entityDataRow;//entityDataRow是class类型，不是so
    public SoDataRow concreteDataRow;
    private object userData;
    
    public UnitBindManager unitBindManager;

    
    public Entity parentEntity;
    [ReadOnly]public int serialId=-1;

    public bool hidden;
    public virtual void Init(EntityDataRow entityDataRow,SoDataRow concreteDataRow,object userData)//传入的是克隆后的，不用担心引用问题
    {
        this.entityDataRow = entityDataRow;
        this.concreteDataRow = concreteDataRow;
        this.userData = userData;

        unitBindManager= GetComponent<UnitBindManager>();
        if (unitBindManager==null)
        {
            unitBindManager = gameObject.AddComponent<UnitBindManager>();
        }

       
        
        
    }

    public virtual void OnShow()
    {
        hidden = false;
    }

    public virtual void OnUpdate()
    {
        
    }
    
    public virtual void OnHide()
    {
        hidden = true;
    }

    /// <summary>
    /// 添加到别人身上
    /// </summary>
    /// <param name="parentEntity"></param>
    public virtual void OnAttachTo(Entity parentEntity)
    {
        this.parentEntity = parentEntity;
    }

    /// <summary>
    /// 别人添加到我身上
    /// </summary>
    /// <param name="childEntity"></param>
    public virtual void OnAttached(Entity childEntity)
    {
        
    }

    /// <summary>
    /// 从我身上移除了childEntity
    /// </summary>
    /// <param name="childEntity"></param>
    public virtual void OnDetached(Entity childEntity)
    {
        
    }

    /// <summary>
    /// 我被parentEntity从身上移除了
    /// </summary>
    /// <param name="parentEntity"></param>
    public virtual void OnDetachFrom(Entity parentEntity)
    {
        this.parentEntity = null;
    }

    public virtual void Hide()
    {
        GameEntry.GetGameComponent<EntityComponent>().HideEntity(this);
    }

    public void Hide(float delay)
    {
        if(hidden)
            return;
        Timer.Register(delay, ()=>
        {
            if (!hidden)
                Hide();
        });
    }
    
}
