using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectEntityDataRow:SoDataRow
{
    public int entityId;
    
    
    
}

[CreateAssetMenu(menuName = "SoDataTable/粒子特效表")]
public class EffectEntityDataTable : SoDataTable<EffectEntityDataRow>
{
    
}
