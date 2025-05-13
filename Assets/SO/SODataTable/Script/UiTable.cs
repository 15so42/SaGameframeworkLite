using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UiDataRow:SoDataRow
{
    public GameObject uiPfb;
    public string uiGroupName="Default";
    //public bool pauseCoveredUiForm;
    [Header("是否控制玩家输入")]
    public bool addInputLock;

    //在ui组中的优先级
    public int priorityInGroup;

}
[CreateAssetMenu(menuName = "SoDataTable/Ui表")]
public class UiTable : SoDataTable<UiDataRow>
{
   
}
