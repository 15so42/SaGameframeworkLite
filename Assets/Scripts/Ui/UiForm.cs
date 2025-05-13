using System.Collections;
using System.Collections.Generic;
using System.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;



public abstract class UiForm : MonoBehaviour
{
    [ReadOnly]
    public UiDataRow uiDataRow;

    [ReadOnly]
    public bool closed;
    public virtual void Init(UiDataRow uiDataRow,object userData=null)
    {
        this.uiDataRow = uiDataRow;
        
    }

    public virtual void OnOpen()
    {
        closed = false;
    }


    public virtual void OnUpdate()
    {
        
    }


    public virtual void OnClose()
    {
        closed = true;
    }


    public virtual void Close()
    {
        GameEntry.GetGameComponent<UiComponent>().CloseUiForm(this);
    }

    public abstract bool HandleEscEvent();
}
