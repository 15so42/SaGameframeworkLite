using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.Serialization;


[System.Serializable]
public struct UiGroupConfig
{
    public string groupName;
    [Header("ui组的基础order")]
    public int baseOrder;

}

public class UiGroup
{
    
    public UiGroupConfig uiGroupConfig;
    public void Init(UiGroupConfig uiGroupConfig)
    {
        this.uiGroupConfig = uiGroupConfig;
    }

    [ReadOnly]
    public List<UiForm> uiForms = new List<UiForm>();

    
    public void OnUpdate()
    {
        foreach (var uiForm in uiForms)
        {
            uiForm.OnUpdate();
        }
    }

    public UiForm GetTopUiForm()
    {
        if (uiForms.Count > 0)
        {
            for (int i = uiForms.Count-1; i >= 0; i--)
            {
                if (uiForms[i] != null && !uiForms[i].closed)
                    return uiForms[i];
            }
        }

        return null;
    }
    
    public void AddUiForm(UiForm uiForm)
    {
        // 先添加到列表中
        uiForms.Add(uiForm);
        uiForm.OnOpen();
    
        // 添加后重新排序
        ResortUiForms();
    }

    public void RemoveUiForm(UiForm uiForm, bool resort = true)
    {
        uiForm.OnClose();
        GameEntry.GetGameComponent<UiComponent>().RecycleUiForm(uiForm);
    
        uiForms.Remove(uiForm);
    
        if (resort)
        {
            ResortUiForms();
        }
    }

    // 根据优先级进行排序，并更新Canvas的sortingOrder
    private void ResortUiForms()
    {
        // 按优先级排序（这里假设数值越大优先级越高，如果逻辑不同，可调整比较器）
        uiForms.Sort((a, b) => a.uiDataRow.priorityInGroup.CompareTo(b.uiDataRow.priorityInGroup));

        // 按照排序结果重新设置排序层级
        for (int i = 0; i < uiForms.Count; i++)
        {
            uiForms[i].GetComponent<Canvas>().sortingOrder = uiGroupConfig.baseOrder + i;
        }
    }


    public bool HandleEscEvent()
    {
        for (int i = uiForms.Count - 1; i >= 0; i--)
        {
            if (uiForms[i].HandleEscEvent())
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveAllUiForm()
    {
        for (int i = 0; i < uiForms.Count; i++)
        {
            RemoveUiForm(uiForms[i],false);
            i--;
        }
    }
}

public class UiComponent : IGameComponent
{

    public Transform root;//存放所有UiForm的Transform
    
    public List<UiGroupConfig> uiGroupConfigs = new List<UiGroupConfig>();

    private List<UiGroup> uiGroups = new List<UiGroup>();
    
    //存放每个uiGroup对应的Transform，添加到UIGroup中的ui会会放到对应的uiGroupRoot中
    private Dictionary<UiGroup, Transform> uiGroupRoots = new Dictionary<UiGroup, Transform>();

    //部分Ui可能会多次使用，所以放入对象池中，只有设置适用对象池的uiForm才会放入
    private Dictionary<int, ObjectPool<GameObject>> uiFormPools = new Dictionary<int, ObjectPool<GameObject>>();
    
    

    //持有所有UiFomrs
    public List<UiForm> uiForms;
    
    //ui堆栈控制玩家输入
    private int inputLockCount;

    public int InputLockCount
    {
        get => inputLockCount;
        set
        {
            inputLockCount = value;
            TypeEventSystem.Send(new InputLockChangeEvent(inputLockCount));
        }
    }

    protected override void Awake()
    {
        base.Awake();
       
        
        foreach (var uiGroupConfig in uiGroupConfigs)
        {
            var uiGroup = new UiGroup();

            uiGroup.Init(uiGroupConfig);
            uiGroups.Add(uiGroup);
            
            var go = new GameObject(uiGroupConfig.groupName);
            go.transform.SetParent(root);
            go.transform.localPosition=Vector3.zero;
            uiGroupRoots.Add(uiGroup,go.transform);

        }

        uiInputActions = new UiInputActions();
    }
   

    private void Update()
    {
        foreach (var uiGroup in uiGroups)
        {
            uiGroup.OnUpdate();
        }
    }

   


    public UiForm GetTopUiForm()
    {
        for (int i = uiGroups.Count; i >= 0; i++)
        {
            var topUiFormInGroup = uiGroups[i].GetTopUiForm();
            if (topUiFormInGroup != null)
            {
                return topUiFormInGroup;
            }
        }

        return null;
    }

    public UiForm ShowUiForm(int id, object userData=null)
    {
        var dataRow = GameEntry.GetGameComponent<SoDataTableComponent>().GetSoDataRow<UiDataRow>(id);
        var uiGroup = uiGroups.Find(x => x.uiGroupConfig.groupName == dataRow.uiGroupName);
        if (uiGroup == null)
        {
            throw new KeyNotFoundException($"没有找到id为{id}的UI的目标UI组");
        }

        GameObject go = null;
        
        if (uiFormPools.ContainsKey(dataRow.id))
        {
            go = uiFormPools[dataRow.id].Get();
        }
        else
        {
            var pool = new ObjectPool<GameObject>(() =>
            {
                var tmpGo = GameObject.Instantiate(dataRow.uiPfb, uiGroupRoots[uiGroup]);
                tmpGo.SetActive(false);
                return tmpGo;
            },null,x => x.SetActive(false),null,true,3);
            uiFormPools.Add(dataRow.id,pool);
            
            go = pool.Get();
        }
        go.SetActive(true);
        
       
        go.transform.localPosition = Vector3.zero;

        var uiForm = go.GetComponent<UiForm>();
        uiForm.Init(dataRow,userData);
        
        uiGroup.AddUiForm(uiForm);
        uiForms.Add(uiForm);

        if (uiForm.uiDataRow.addInputLock)
            InputLockCount++;
        

        return uiForm;
    }

    public void CloseUiForm(UiForm uiForm)
    {
        var uiGroupName = uiForm.uiDataRow.uiGroupName;
        var uiGroup = uiGroups.Find(x => x.uiGroupConfig.groupName == uiGroupName);
        if (uiGroup!=null)
        {
            uiGroup.RemoveUiForm(uiForm);
            uiForms.Remove(uiForm);
            if (uiForm.uiDataRow.addInputLock)
                InputLockCount--;
        }
       
    }

    public void CloseAllUiForm()
    {
        foreach (var uiGroup in uiGroups)
        {
            uiGroup.RemoveAllUiForm();
        }

        inputLockCount = 0;
        uiForms.Clear();
    }

    public void RecycleUiForm(UiForm uiForm)
    {
        //在生成uiForm的时候就已经生成了pool，所以不用再检查了
        if (uiFormPools.ContainsKey(uiForm.uiDataRow.id))
        {
            //Debug.Log("ReleaseUiForm"+uiForm.uiDataRow.id);
            uiFormPools[uiForm.uiDataRow.id].Release(uiForm.gameObject);

        }
    }


    private UiInputActions uiInputActions;
    private void OnEnable()
    {
        uiInputActions.Enable();
        uiInputActions.Ui.Esc.performed += HandleEscEvent;
    }

    private void OnDisable()
    {
        uiInputActions.Disable();
        uiInputActions.Ui.Esc.performed -= HandleEscEvent;
    }


    void HandleEscEvent(InputAction.CallbackContext callbackContext)
    {
        //Debug.LogError("触发返回操作！！");
        if (!HandleEscEvent())
        {
            ShowUiForm(6);
        }
    }
    
    /// <summary>
    /// bool 表示本次Esc是否成功处理了某个UI，比如说是否成功关闭了某个UI，如果为false表示没有需要关闭的ui
    /// </summary>
    /// <returns></returns>
    public bool HandleEscEvent()
    {
        //处理Esc事件
        for (int i = uiGroups.Count - 1; i >= 0; i--)
        {
            if (uiGroups[i].HandleEscEvent())
            {
                return true;
            }
        }

        return false;
    }
}
