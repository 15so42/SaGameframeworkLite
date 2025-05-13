using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public enum RelationType
{
    Friendly,  // 友军
    Neutral,   // 中立
    Hostile      // 敌人
}


public class CampManager
{
    
}

public class CombatComponent : IGameComponent
{

    #region 阵营处理

    // 存储所有已注册的阵营
    private readonly HashSet<string> registeredCamps = new HashSet<string>();
    
    // 关系表：camp -> (targetCamp -> relation)
    private readonly Dictionary<string, Dictionary<string, RelationType>> relations = 
        new Dictionary<string, Dictionary<string, RelationType>>();

    // 注册新阵营（自动设置自己阵营为友军）
    public void RegisterCamp(string camp)
    {
        if (registeredCamps.Add(camp))
        {
            // 初始化关系表
            relations[camp] = new Dictionary<string, RelationType>
            {
                [camp] = RelationType.Friendly  // 同阵营默认友军
            };
            
            campColors.Add(camp,GetColorByIndex(registeredCamps.Count-1));
        }
    }

    // 设置双方为友军
    public void BeFriends(string campA, string campB)
    {
        ValidateCampsExist(campA, campB);
        SetMutualRelation(campA, campB, RelationType.Friendly);
    }

    // 设置双方为中立
    public void BeNeutrals(string campA, string campB)
    {
        ValidateCampsExist(campA, campB);
        SetMutualRelation(campA, campB, RelationType.Neutral);
    }

    // 设置双方为敌人
    public void BeEnemies(string campA, string campB)
    {
        ValidateCampsExist(campA, campB);
        SetMutualRelation(campA, campB, RelationType.Hostile);
    }

    // 获取双方关系（优先查询明确设定的关系）
    public RelationType GetRelation(string campA, string campB)
    {
        if (campA == campB) return RelationType.Friendly;
        
        // 尝试获取A对B的关系
        if (relations.TryGetValue(campA, out var campARelations) &&
            campARelations.TryGetValue(campB, out var relation))
        {
            return relation;
        }
        
        // 默认返回中立
        return RelationType.Neutral; 
    }

    
    // 设置双向关系
    private void SetMutualRelation(string campA, string campB, RelationType type)
    {
        relations[campA][campB] = type;
        relations[campB][campA] = type;
    }

    // 验证阵营是否已注册
    private void ValidateCampsExist(params string[] camps)
    {
        foreach (var camp in camps)
        {
            if (!registeredCamps.Contains(camp))
            {
                throw new System.ArgumentException($"Camp {camp} is not registered!");
            }
        }
    }
    #endregion


    
    
    
    public List<Color> colors;

    public Dictionary<string, Color> campColors = new Dictionary<string, Color>();
    //获取颜色，基本就是阵营的颜色
    public Color GetColorByIndex(int index)
    {
        if (index < colors.Count)
        {
            return colors[index];
        }

        return Color.black;

    }

    public Color GetCampColor(string camp)
    {
        if (campColors.ContainsKey(camp))
        {
            return campColors[camp];
        }

        return Color.black;
    }




    public DamageManager damageManager;
    public TimelineManager timelineManager;


    


    protected override void Awake()
    {
        base.Awake();
        damageManager = new DamageManager();
        timelineManager = new TimelineManager();
    }


    private void OnEnable()
    {
        damageManager.onHandleDamage += HandleDamage;
    }

    private void OnDisable()
    {
        damageManager.onHandleDamage -= HandleDamage;
    }

    void HandleDamage(DamageInfo damageInfo,int dValue,bool isHeal)
    {
        Transform bpTransform = null;
        var ubm = damageInfo.defender.GetComponent<UnitBindManager>();
        if (ubm)
        {
            bpTransform = ubm.GetBindPointByKey("FlyText").transform;
        }
        GameEntry.FlyText.FlyText(bpTransform.position,isHeal?$"<color=green>{-dValue}</color>":$"<color=red>{dValue}</color>");
        
        TypeEventSystem.Send(new DoDamageEvent(damageInfo,dValue,isHeal));
    }
    
    


    public void CreateDamage(GameObject attacker,GameObject victim,Damage damage, float criticalRate=0f, DamageInfoTag[] tags=null)
    {
        Debug.Log($"{attacker}对{victim}造成伤害：{damage.melee},{damage.bullet}，{damage.explosion},{damage.mental}",attacker.gameObject);
        damageManager.DoDamage(attacker,victim,damage,criticalRate,tags);
    }
    
    

    //特效管理器
    private Dictionary<string, GameObject> sightEffect = new Dictionary<string, GameObject>();
    

    private void FixedUpdate() {
        //管理一下视觉特效，看哪些需要清楚了
        List<string> toRemoveKey = new List<string>();
        foreach(KeyValuePair<string, GameObject> se in sightEffect){
            if (se.Value == null) toRemoveKey.Add(se.Key);
        }
        for (int i = 0; i < toRemoveKey.Count; i++) sightEffect.Remove(toRemoveKey[i]);
        
        
        
        timelineManager.FixedUpdate();
        damageManager.FixedUpdate();
    }
    

    ///<summary>
    ///创建一个子弹对象在场景上
    ///<param name="bulletLauncher">子弹发射器</param>
    ///</summary>
    public BulletEntity CreateBullet(BulletLauncher bulletLauncher,GameObject[] targets=null)
    {

       
        var bulletEntity = GameEntry.Entity.ShowBulletEntity(bulletLauncher.model.bulletEntityId,
            bulletLauncher.firePosition, bulletLauncher);//旋转不重要，因为旋转生成后在Init方法中处理
        
       
       bulletEntity.gameObject.GetOrAddComponent<BulletState>().InitByBulletLauncher(
           bulletLauncher, 
           targets
       );

       return bulletEntity;
    }

    ///<summary>
    ///删除一个存在的子弹Object
    ///<param name="aoe">子弹的GameObject</param>
    ///<param name="immediately">是否当场清除，如果false，就是把时间变成0</param>
    ///</summary>
    public void RemoveBullet(BulletState bullet, bool immediately = false){
        if (!bullet) return;
        BulletState bulletState = bullet;
        if (!bulletState) return;
        bulletState.duration = 0;
        if (immediately == true){//设置为0后会自动移除，immediately是强行移除
            if (bulletState.model.onRemoved != null){
                bulletState.model.onRemoved(bullet.gameObject);
            }
            GameEntry.Entity.HideEntity(bullet.GetComponent<Entity>());
        }
    }

    ///<summary>
    ///创建一个aoe对象在场景上
    ///<param name="aoeLauncher">aoe的创建信息</param>
    ///</summary>
    public AoeEntity CreateAoE(AoeLauncher aoeLauncher){
        
        
        var aoeEntity = GameEntry.Entity.ShowAoeEntity(aoeLauncher.model.aoeEntityTypeId, null);
        aoeEntity.GetOrAddComponent<AoeState>().InitByAoeLauncher(aoeLauncher);
        return aoeEntity;
    }

  

    ///<summary>
    ///删除一个存在的aoeObject
    ///<param name="aoe">aoe的GameObject</param>
    ///<param name="immediately">是否当场清除，如果false，就是把时间变成0</param>
    ///</summary>
    public void RemoveAoE(GameObject aoe, bool immediately = false){
        if (!aoe) return;
        AoeState aoeState = aoe.GetComponent<AoeState>();
        if (!aoeState) return;
        aoeState.duration = 0;
        if (immediately == true){    
            if (aoeState.model.onRemoved != null){
                aoeState.model.onRemoved(aoe);
            }
            GameEntry.Entity.HideEntity(aoe.GetComponent<Entity>());
        }
    }

    ///<summary>
    ///创建一个视觉特效在场景上
    ///<param name="effectId">EffectId</param>
    ///<param name="pos">创建的位置</param>
    ///<param name="rotation">旋转</param>
    ///<param name="key">特效的key，如果重复则无法创建，删除的时候也有用，空字符串的话不加入管理</param>
    ///<param name="loop">是否循环，循环的得手动remove</param>
    ///<param name="duration">持续时间</param>
    ///</summary>
    public void CreateSightEffect(int effectId, Vector3 pos, Quaternion rotation, string key = "", bool loop = false,float duration=1){
        if (sightEffect.ContainsKey(key) == true) return;    //已经存在，加不成
        
        var effectGo = GameEntry.Entity.ShowEffectEntity(effectId, pos, rotation, null);
        
        effectGo.transform.SetParent(transform);
        
        if (!effectGo) return;
        SightEffect se = effectGo.AddComponent<SightEffect>();
        if (!se){
            GameEntry.Entity.HideEntity(effectGo);
            return;
        }

        se.duration = duration;
        if (loop == false){
            effectGo.Hide(se.duration);
        }

        if (key != "")  sightEffect.Add(key, effectGo.gameObject);
       
    }

    ///<summary>
    ///删除一个视觉特效在场景上
    ///<param name="key">特效的key</param>
    ///</summary>
    public void RemoveSightEffect(string key){
        if (sightEffect.ContainsKey(key) == false) return;
        //Destroy(sightEffect[key]);
            GameEntry.Entity.HideEntity(sightEffect[key].GetComponent<Entity>());
        sightEffect.Remove(key);
    }

    

    public void CreateCharacter(int typeId,string camp,Vector3 pos,ChaProperty baseProp,string[] tags = null,AddBuffInfo[] addBuffs=null,GameObject caster=null)
    {
        var battleEntity =
            GameEntry.Entity.ShowBattleEntity(typeId, pos, Quaternion.identity, new BattleEntityUserData(camp));

        battleEntity.chaState.InitBaseProp(baseProp);
        battleEntity.chaState.tags = tags;
                    
        for (int i = 0; i < addBuffs.Length; i++){
            addBuffs[i].caster = caster;
            addBuffs[i].target = battleEntity.gameObject;
            battleEntity.chaState.AddBuff(addBuffs[i]);
        }
    }
    
    public void CreateTimeline(TimelineModel timelineModel, GameObject caster, object param){
        timelineManager.AddTimeline(timelineModel, caster, param);
    }
    public void CreateTimeline(TimelineObj timeline){
        timelineManager.AddTimeline(timeline);
    }
    
    public  AoeEntity CreateAoe(string id,GameObject caster,Vector3 pos,float duration,Quaternion rotation,float scale,string aoeTweenId,object[] tweenParam)
    {
        var aoeLauncher = new AoeLauncher(DesingerTables.AoE.data[id], caster,
            pos, duration, rotation,scale,string.IsNullOrEmpty(aoeTweenId)?null: DesignerScripts.AoE.aoeTweenFunc[aoeTweenId], tweenParam);
        return GameEntry.Combat.CreateAoE(aoeLauncher);
    }
   
    public  AoeEntity CreateAoe(string id,GameObject caster,Vector3 pos,float duration,Quaternion rotation,float scale,string aoeTweenId,object[] tweenParam,Dictionary<string,object> aoeParam)
    {
        var aoeLauncher = new AoeLauncher(DesingerTables.AoE.data[id], caster,
            pos, duration, rotation,scale,string.IsNullOrEmpty(aoeTweenId)?null: DesignerScripts.AoE.aoeTweenFunc[aoeTweenId], tweenParam,aoeParam);
        return GameEntry.Combat.CreateAoE(aoeLauncher);
    }

    public  void CreateDirectDamage(GameObject attacker,GameObject victim, Damage damage)
    {
        GameEntry.Combat.CreateDamage(attacker,victim,damage, 
            0.05f,
            new DamageInfoTag[]{DamageInfoTag.directDamage, });
    }

    public Damage CreateDamageStruct(int melee, int bullet, int explosion, int mental)
    {
        return new Damage(melee, bullet, explosion, mental);
    }
}