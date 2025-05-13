using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArmorEntityDataRow : SoDataRow
{
    public int entityId;
    
    
    public string armorName;
   
    [Header("属性变化")]
    public int hpAdd=0;
    public int mpAdd=0;
    public int attackAdd=0;
    public int defenseAdd=0;
    public int moveSpeedAdd=0;
    public int actionSpeedAdd=0;
    
    public int hpTimes=0;
    public int mpTimes=0;
    public int attackTimes=0;
    public int defenseTimes=0;
    public int moveSpeedTimes=0;
    public int actionSpeedTimes=0;
    
    
    public string[] buffs;
    
}
[CreateAssetMenu(menuName = "SoDataTable/Armor表")]
public class ArmorEntityTable : SoDataTable<ArmorEntityDataRow>
{
   
}
