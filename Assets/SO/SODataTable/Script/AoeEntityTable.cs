using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AoeEntityDataRow : SoDataRow
{
    public int entityId;
}
[CreateAssetMenu(menuName = "SoDataTable/Aoe表")]
public class AoeEntityTable : SoDataTable<AoeEntityDataRow>
{
   
}
