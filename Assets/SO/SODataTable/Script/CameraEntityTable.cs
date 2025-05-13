using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CameraEntityDataRow : SoDataRow
{
    public int entityId;
    public int basePriority = 0;
}

[CreateAssetMenu(menuName = "SoDataTable/CameraEntity表" )]
public class CameraEntityTable : SoDataTable<CameraEntityDataRow>
{
  
}
