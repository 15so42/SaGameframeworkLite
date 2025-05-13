using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum MoveMethod
{
    RigidbodyVelocity,
    TranslatePosWithPenetration,
    RigidbodyPos,
    TranslatePos,
}

public enum RotateMethod
{
    Slerp,
    Fixed,
    RigidbodySlerp,
}

[System.Serializable]
public class BattleEntityDataRow:SoDataRow
{
    public int entityId;

    public int hp=100;
    public int mp=100;
    public int baseDamage=5;
    public int defense=0;
    public int moveSpeed=1;
    public int actionSpeed=1;

    public MoveMethod moveMethod;
    [Header("MoveMethod要用到的变量，比如加速度")]
    public float moveMethodSpeedParam=10;
    [Header("rotateMethod要用到的变量，比如Slerp速度和FixedMethod中的旋转速度")]
    public RotateMethod rotateMethod;
    public float rotateMethodParam=10;
    

    public int weaponId;
    public int armorId;
   
    [Header("自带技能")]
    public string[] skills;
    [Header("自带buff，注意，因为是自带的，所以时间设置为永久，这里添加的buff都是永久时间的buff")]
    public string[] buffs;
    
    public string[] tags;
}

[CreateAssetMenu(menuName = "SoDataTable/战斗单位表")]
public class BattleEntityDataTable : SoDataTable<BattleEntityDataRow>
{
    
}
