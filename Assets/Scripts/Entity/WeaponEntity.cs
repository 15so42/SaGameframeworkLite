using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEntity : Entity
{
    public WeaponEntityDataRow weaponEntityDataRow;
    public override void Init(EntityDataRow entityDataRow, SoDataRow concreteDataRow, object userData)
    {
        base.Init(entityDataRow, concreteDataRow, userData);
        this.weaponEntityDataRow=concreteDataRow as WeaponEntityDataRow;
        
    }

    private int curIndex;

    public string GetSkill()
    {
        var skills = weaponEntityDataRow.skills;
        if (skills == null || skills.Length == 0) 
            return string.Empty;

        if (weaponEntityDataRow.skillPickType == SkillPickType.InOrder)
        {
            // 顺序模式：简单循环
            curIndex = (curIndex + 1) % skills.Length;
            return skills[curIndex];
        }
        else if (weaponEntityDataRow.skillPickType == SkillPickType.OutOfOrder)
        {
            // 乱序模式：纯随机（允许重复）
            return skills[Random.Range(0, skills.Length)];
        }
    
        return string.Empty;
    }
    public void Fire()
    {
        var skillId = GetSkill();
        var owner = parentEntity as BattleEntity;
        if (owner != null) owner.chaState.CastSkill(skillId);
    }
}
