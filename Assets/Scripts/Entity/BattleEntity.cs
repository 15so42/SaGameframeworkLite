using System;
using System.Collections;
using System.Collections.Generic;
using DesingerTables;
using Unity.VisualScripting;
using UnityEngine;


public class BattleEntityUserData
{
    public string camp;

    public BattleEntityUserData(string camp)
    {
        this.camp = camp;
    }
}
public class BattleEntity : Entity
{
    private BattleEntityDataRow battleEntityDataRow;


    public ChaState chaState;

    private BattleEntityUserData battleEntityUserData;

    public WeaponEntity weaponEntity;
    public ArmorEntity armorEntity;

    public CameraFollowAndLookConfig cameraFollowAndLookConfig;


    private  BattleEntityDieStrategy battleEntityDieStrategy;

    public override void Init(EntityDataRow entityDataRow,SoDataRow concreteDataRow, object userData)
    {
        base.Init(entityDataRow, concreteDataRow,userData);
        
        battleEntityDataRow=concreteDataRow as BattleEntityDataRow;
        
        battleEntityUserData=userData as BattleEntityUserData;
        
        cameraFollowAndLookConfig = gameObject.GetOrAddComponent<CameraFollowAndLookConfig>();
        
        chaState = gameObject.GetOrAddComponent<ChaState>();
        
        

        if (battleEntityUserData != null) chaState.Camp = battleEntityUserData.camp;


        chaState.tags = battleEntityDataRow.tags;
       
        ChaProperty chaProperty = new ChaProperty(battleEntityDataRow.moveSpeed, battleEntityDataRow.hp,
            battleEntityDataRow.mp, battleEntityDataRow.baseDamage, battleEntityDataRow.defense
            , battleEntityDataRow.actionSpeed);
        
        chaState.Init();
        chaState.GetUnitMove().Init(battleEntityDataRow.moveMethod,battleEntityDataRow.moveMethodSpeedParam);
        chaState.GetUnitRotate().Init(battleEntityDataRow.rotateMethod,battleEntityDataRow.rotateMethodParam);
        chaState.InitBaseProp(chaProperty);
        foreach (var skillId in battleEntityDataRow.skills)
        {
            chaState.LearnSkill(DesingerTables.Skill.data[skillId]);
        }

        foreach (var buffId in battleEntityDataRow.buffs)
        {
            chaState.AddBuff(new AddBuffInfo(DesingerTables.Buff.data[buffId],gameObject,gameObject,1,1,true,true));
        }

        var weaponPoint= unitBindManager.GetBindPointByKey(GameEntry.Const.CONST_BPKey_WeaponPoint);
        var weaponPointTransform = transform;
        if (weaponPoint != null)
        {
            weaponPointTransform = weaponPoint.transform;
        }
        


        #region Weapon 武器添加
        weaponEntity = GameEntry.Entity.ShowWeaponEntity(battleEntityDataRow.weaponId, this, weaponPointTransform);
        //学习武器的技能
        var weaponDataRow = weaponEntity.weaponEntityDataRow;
        chaState.weaponsProps.Add(new EquipmentProps(new ChaProperty[]{new ChaProperty(weaponDataRow.moveSpeedAdd,weaponDataRow.hpAdd,weaponDataRow.mpAdd,weaponDataRow.attackAdd,weaponDataRow.defenseAdd,weaponDataRow.actionSpeedAdd),
            new ChaProperty(weaponDataRow.moveSpeedTimes,weaponDataRow.hpTimes,weaponDataRow.mpTimes,weaponDataRow.attackTimes,weaponDataRow.defenseTimes,weaponDataRow.actionSpeedTimes)}));

        foreach (var skillId in weaponDataRow.skills)
        {
            chaState.LearnSkill(DesingerTables.Skill.data[skillId]);
        }
        foreach (var buffId in weaponDataRow.buffs)
        {
            chaState.AddBuff(new AddBuffInfo(DesingerTables.Buff.data[buffId],gameObject,gameObject,1,1,true,true));
        }

        #endregion
        
        #region Armor 防具添加
        var armorPoint= unitBindManager.GetBindPointByKey(GameEntry.Const.CONST_BPKey_ArmorPoint);
        var armorPointTransform = transform;
        if (armorPoint != null)
        {
            armorPointTransform = armorPoint.transform;
        }
        armorEntity = GameEntry.Entity.ShowArmorEntity(battleEntityDataRow.armorId, this, armorPointTransform);
        //添加防具的buff
        var armorDataRow = armorEntity.armorEntityDataRow;
        chaState.armorsProps.Add(new EquipmentProps(new ChaProperty[]{new ChaProperty(armorDataRow.moveSpeedAdd,armorDataRow.hpAdd,armorDataRow.mpAdd,armorDataRow.attackAdd,armorDataRow.defenseAdd,armorDataRow.actionSpeedAdd),
            new ChaProperty(armorDataRow.moveSpeedTimes,armorDataRow.hpTimes,armorDataRow.mpTimes,armorDataRow.attackTimes,armorDataRow.defenseTimes,armorDataRow.actionSpeedTimes)}));
        
        foreach (var buffId in armorDataRow.buffs)
        {
            chaState.AddBuff(new AddBuffInfo(DesingerTables.Buff.data[buffId],gameObject,gameObject,1,1,true,true));
        } 

        #endregion
        
        //死亡处理
        battleEntityDieStrategy = gameObject.GetOrAddComponent<BattleEntityDieStrategy>();

    }

    public override void OnShow()//Init后执行
    {
        base.OnShow();
        chaState.onDead += OnDie;
    }

    public virtual void OnDie()
    {
        battleEntityDieStrategy.Die(this);
        
    }

    public override void OnHide()
    {
        base.OnHide();
        chaState.onDead -= OnDie;
    }

    public void Move(Vector3 velocity)
    {
        chaState.OrderMove(velocity);
    }
    
    public void Rotate(Quaternion targetRotation)
    {
        chaState.OrderRotateTo(targetRotation);
    }
}
