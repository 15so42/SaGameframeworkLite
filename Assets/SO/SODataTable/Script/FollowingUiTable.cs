using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FollowingUiDataRow:SoDataRow
{
    public GameObject pfb;
    public string bindPointKey;
}
[CreateAssetMenu(menuName = "SoDataTable/FollowingUiTable")]
public class FollowingUiTable : SoDataTable<FollowingUiDataRow>
{
    
}
