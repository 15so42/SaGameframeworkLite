using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BulletEntityDataRow : SoDataRow
{
    public int entityId;
}
[CreateAssetMenu(menuName = "SoDataTable/Bullet表")]
public class BulletEntityTable : SoDataTable<BulletEntityDataRow>
{
   
}
