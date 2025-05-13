using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;



///<summary>
///子弹的“状态”，用来管理当前应该怎么移动、应该怎么旋转、应该怎么播放动画的。
///是一个角色的总的“调控中心”。
///</summary>
public class BulletState:MonoBehaviour{
    ///<summary>
    ///这是一颗怎样的子弹
    ///</summary>
    public BulletModel model;
    
    ///<summary>
    ///要发射子弹的这个人的gameObject，这里就认角色（拥有ChaState的）
    ///当然可以是null发射的，但是写效果逻辑的时候得小心caster是null的情况
    ///</summary>
    public GameObject caster;

    ///<summary>
    ///子弹发射时候，caster的属性，如果caster不存在，就会是一个ChaProperty.zero
    ///在一些设计中，比如wow的技能中，技能效果是跟发出时候的角色状态有关的，之后即使获得或者取消了buff，更换了装备，数值一样不会受到影响，所以得记录这个释放当时的值
    ///</summary>
    public ChaProperty propWhileCast = ChaProperty.zero;

    ///<summary>
    ///发射的角度，单位：角度，如果useFireDegreeForever == true，那就得用这个角度来获得实时飞行路线了
    ///</summary>
    public Quaternion fireRotation;

    ///<summary>
    ///子弹的初速度，单位：米/秒，跟Tween结合获得Tween得到当前移动速度
    ///</summary>
    public float speed;

    ///<summary>
    ///子弹的生命周期，单位：秒
    ///</summary>
    public float duration;

    ///<summary>
    ///子弹已经存在了多久了，单位：秒
    ///毕竟duration是可以被重设的，比如经过一个aoe，生命周期减半了
    ///</summary>
    public float timeElapsed = 0;

   
    ///<summary>
    ///子弹的轨迹函数
    ///<param name="t">子弹飞行了多久的时间点，单位秒。</param>
    ///<return>返回这一时间点上的速度和偏移，Vector3就是正常速度正常前进</return>
    ///</summary>
    public BulletTween tween = null;

    public BulletTweenCondition bulletTweenCondition;
    
    ///<summary>
    ///本帧的移动
    ///</summary>
    private Vector3 velocity = new Vector3();

    ///<summary>
    ///本帧的移动信息
    ///</summary>
    public Vector3 Velocity => velocity;

  
    
    
    ///<summary>
    ///子弹计算移动轨迹是否严格使用发射出来的角度,为true的话子弹方向按照初始方向（fireRotation）进行偏移，否则子弹方向根据子弹实时朝向偏移
    ///</summary>
    public bool useFireDegreeForever = false;

    ///<summary>
    ///子弹命中纪录
    ///</summary>
    public List<BulletHitRecord> hitRecords = new List<BulletHitRecord>();

    ///<summary>
    ///子弹创建后多久是没有碰撞的，这样比如子母弹之类的，不会在创建后立即命中目标，但绝大多子弹还应该是0的
    ///单位：秒
    ///</summary>
    public float canHitAfterCreated = 0;

    ///<summary>
    ///子弹正在追踪的目标，不太建议使用这个，最好保持null
    ///</summary>
    public GameObject followingTarget = null;

    ///<summary>
    ///子弹传入的参数，逻辑用的到的临时记录
    ///</summary>
    public Dictionary<string, object> param = new Dictionary<string, object>();
    


    ///<summary>
    ///还能命中几次
    ///</summary>
    public int hp = 1;

    private MoveMethod moveMethod;
   

    
    private UnitRotate unitRotate;
    private UnitMove unitMove;
    //private GameObject viewContainer;

    private Rigidbody rb;
    private Collider[] colliders;

    private bool hitCaster;
    private bool hitAlly;
    private bool hitFoe = true;
    private void Start() {
        
        synchronizedUnits();
    }

    

    ///<summary>
    ///传入的mf是偏移量，函数中现根据是否使用初始角度和elpasedTime计算初始移动方向，然后把mf作为偏移加载初始移动方向上，最后再进行移动，
    /// 我推测这个偏移量可以在之后用来制作散弹之类的地方派上用场。
    ///</summary>
    public void SetMoveForce(Vector3 mf,float deltaTime){
        this.velocity = mf;//表示目标移动向量
        
        
        Quaternion baseMoveRotation=(useFireDegreeForever == true ||
                            timeElapsed <= 0     //还是那个问题，unity的动画走的是update，所以慢了，旋转没转到预设角度，fireRotation
            ) ? fireRotation : transform.rotation; //欧拉获得的是角度

        velocity = baseMoveRotation * this.velocity;
            
        velocity *= speed;
        Debug.DrawRay(transform.position,velocity*10,Color.red);
        unitMove.MoveBy(velocity,deltaTime);
        if(velocity!=Vector3.zero)
            unitRotate.RotateTo(Quaternion.LookRotation(velocity),deltaTime);
    }

   

    ///<summary>
    ///根据BulletLauncher初始化这个数据
    ///<param name="bullet">bulletLauncher</param>
    ///<param name="targets">子弹允许跟踪的全部目标，在这里根据脚本筛选</param>
    ///</summary>
    public void InitByBulletLauncher(BulletLauncher bullet, GameObject[] targets){
        this.model = bullet.model;
        this.caster = bullet.caster;
        if (this.caster && caster.GetComponent<ChaState>()){
            this.propWhileCast = caster.GetComponent<ChaState>().property;
        }
        this.fireRotation = caster.transform.rotation * bullet.localRotation;
        transform.rotation = fireRotation;
        this.speed = bullet.speed;
        this.duration = bullet.duration;
        this.timeElapsed = 0;
        this.tween = bullet.tween;
        this.bulletTweenCondition = bullet.bulletTweenCondition;
        this.useFireDegreeForever = bullet.useFireDegreeForever;
        this.canHitAfterCreated = bullet.canHitAfterCreated;
        this.hitCaster = bullet.hitCaster;
        this.hitAlly = bullet.hitAlly;
        this.hitFoe = bullet.hitFoe;
        this.moveMethod = bullet.model.moveMethod;
        this.hp = bullet.model.hitTimes;

        this.param = new Dictionary<string, object>();
        if (bullet.param != null){
            foreach(KeyValuePair<string, object> kv in bullet.param){
                this.param.Add(kv.Key, kv.Value);
            }
        }
        
        synchronizedUnits();

        this.rb.useGravity = bullet.model.useGravity;
        foreach (var c in this.colliders)
        {
            c.isTrigger = bullet.model.isTrigger;
        }
        
        
        this.followingTarget = bullet.targetFunc == null ? null :
            bullet.targetFunc(this.gameObject, targets);

        //对象池数据清零
        hitRecords.Clear();
        
    }

    //同步一下unitMove和自己的一些状态
    private void synchronizedUnits(){
        if (!unitRotate) unitRotate = gameObject.GetOrAddComponent<UnitRotate>();
        if (!unitMove)  unitMove = gameObject.GetOrAddComponent<UnitMove>();
        
        unitMove.Init(moveMethod,model.moveSpeedParam);

        colliders = gameObject.GetComponentsInChildren<Collider>();
        rb = gameObject.GetOrAddComponent<Rigidbody>();
        rb.collisionDetectionMode =CollisionDetectionMode.Continuous;

        if (colliders == null || colliders.Length==0)
        {
            Debug.LogWarning($"注意,{gameObject.name}没有配置碰撞体");
        }

        if (rb == null)
        {
            Debug.LogWarning($"注意,{gameObject.name}没有配置Rigidbody");
        }
    }

    public void SetMoveMethod(MoveMethod toType){
        this.moveMethod = toType;
        synchronizedUnits();
    }

   

    ///<summary>
    ///添加命中纪录
    ///<param name="target">目标GameObject</param>
    ///</summary>
    public void AddHitRecord(GameObject target){
        hitRecords.Add(new BulletHitRecord(
            target,
            this.model.sameTargetDelay
        ));
    }

    

    private void FixedUpdate()
    {
        float timePassed = Time.fixedDeltaTime;

        //
        // if (hp <= 0) //hp表示的是击中次数，小于等于0表示击中次数用完了
        // {
        //    
        //     return;
        // }
        
        //刚刚创建
        if (timeElapsed <= 0 && model.onCreate != null){

          
            
            model.onCreate(this);
        }
        
        //处理子弹命中记录
        for (int i = 0; i < hitRecords.Count; i++)
        {
            hitRecords[i].timeToCanHit -= timePassed;//同一个单位子弹的命中间隔
            if (hitRecords[i].timeToCanHit <= 0 || hitRecords[i].target == null)
            {
                hitRecords.RemoveAt(i);
                i--;
            }
        }

        if (bulletTweenCondition == null)
        {
            //Debug.Log("不Set");
            unitMove.canMove = false;
        }
        else
        {
            if (bulletTweenCondition(timeElapsed, this, followingTarget))
            {
                //处理子弹移动
                unitMove.canMove = true;
                SetMoveForce(tween == null ? Vector3.forward : tween(timeElapsed, this, followingTarget),Time.fixedDeltaTime);
                //Debug.Log("SetMoveForce");
            }
            else
            {
                unitMove.canMove = false;
                
            }
        }
        
        
        //处理子弹诞生后多久可以碰撞
        if (canHitAfterCreated > 0) {
            canHitAfterCreated -= timePassed;  
        }
        
        //duration处理
        duration -= timePassed;
        timeElapsed += timePassed;
        if (duration <=0 || hp<=0)//到期或者碰到了障碍物就该销毁了，障碍物是指除了chaObj外的碰撞体
        {
            if (model.onRemoved != null)
            {
                model.onRemoved(gameObject);
            }
            var entity=GetComponent<Entity>();

            //Debug.Log(gameObject.name+ entity.serialId+"子弹回收！！！"+duration+","+hp+","+timeElapsed);
            GetComponent<Entity>().Hide();
        }
        
       
    }

    #region 碰撞处理,根据设置的trigger和rigidbody会触发不同的碰撞逻辑，但是碰撞处理是一致的

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("碰撞到collision"+collision.collider+",canHitAfterCreated"+canHitAfterCreated);
        HandleHit(collision.collider);
    }


    public void HandleHit(Collider collider)
    {
        
        if (canHitAfterCreated > 0) return;
        
        for (int i = 0; i < this.hitRecords.Count; i++){//碰撞过的短时间内不再碰撞
            if (hitRecords[i].target == collider.gameObject){
                return;
            }
        }
        
        if(collider.gameObject==caster && hitCaster==false)
            return;
        
        ChaState targetChaState = collider.GetComponentInParent<ChaState>();
        

        if (targetChaState)
        {
            if(targetChaState.immuneTime>0)//不对无敌的敌人进行碰撞
                return;
            
            var casterChaState = caster.GetComponent<ChaState>();
            var relation = GameEntry.Combat.GetRelation(casterChaState.Camp, targetChaState.Camp);
            //阵营检测
            if (hitCaster && casterChaState == targetChaState)
            {
                //不return 
            }
            else
            {
                if ((hitAlly == false && relation ==RelationType.Friendly) ||
                    (hitFoe == false && relation ==RelationType.Hostile))
                {
                    //Debug.LogError($"弹开,{collider.gameObject.name},hitAlly:{hitAlly},hitFoe:{hitFoe},relation:{relation}");
                    return;
                }
            }
           
            
            

            if (model.onHit != null)
            {
                Debug.Log("子弹OnHit"+targetChaState.gameObject.name);
                model.onHit(this, targetChaState);
            }
            
           
            if (hp > 0)//还能继续碰撞
            {
                AddHitRecord(gameObject);
            }
            hp --;

        }
        
     
    }
    

    #endregion
    
}