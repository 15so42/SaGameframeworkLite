using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityDataRow:SoDataRow
{
    public GameObject pfb;
    public bool usePool;
    
    public object Clone()
    {
        return new EntityDataRow
        {
            pfb = this.pfb,       // 引用类型，根据需求可 Instantiate
            usePool = this.usePool
        };
    }

}
[CreateAssetMenu(menuName = "SoDataTable/Entity表")]
public class EntityTable : SoDataTable<EntityDataRow>
{
}
