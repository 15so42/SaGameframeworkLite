using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillPickType
{
    InOrder,//顺序
    OutOfOrder,//乱序
}

[Serializable]
public class WeaponEntityDataRow:SoDataRow
{
    public int entityId;
    public string weaponName;
   
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

    public SkillPickType skillPickType;
    [Header("可以有多个，本项目只使用第一个")]
    public string[] skills;
    
    public string[] buffs;
    
    
}

[CreateAssetMenu(menuName = "SoDataTable/WeaponEntity表")]
public class WeaponEntityTable : SoDataTable<WeaponEntityDataRow>
{
    
}
