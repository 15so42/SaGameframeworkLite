using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

///<summary>
///在一个gameObject下添加这个，让这个gameObject成为一个“绑点”，这样就可以在这东西里面管理一些挂载的gameObject
///最常见的用途是角色身上某个点播放视觉特效什么的。
///</summary>
public class UnitBindPoint : MonoBehaviour{
    ///<summary>
    ///绑点的名称
    ///</summary>
    public string key;

    ///<summary>
    ///偏移坐标
    ///</summary>
    public Vector3 offset;

    ///<summary>
    ///已经挂着的gameobject信息
    ///key就是一个索引，便于找到
    ///</summary>
    private Dictionary<string, BindGameObjectInfo> bindGameObject = new Dictionary<string, BindGameObjectInfo>();

    private void FixedUpdate() {
        List<string> toRemove = new List<string>();
        foreach(KeyValuePair<string, BindGameObjectInfo> goInfo in bindGameObject){
            if (goInfo.Value.effectEntity == null){
                toRemove.Add(goInfo.Key);
                continue;
            }
            if (goInfo.Value.forever == false){
                goInfo.Value.duration -= Time.fixedDeltaTime;
                if (goInfo.Value.duration <= 0){
                    //Destroy(goInfo.Value.gameObject);
                    GameEntry.Entity.HideEntity(goInfo.Value.effectEntity.GetComponent<Entity>());
                    toRemove.Add(goInfo.Key);
                }
            }
        }
        for (int i = 0; i < toRemove.Count; i++){
            bindGameObject.Remove(toRemove[i]);
        }
    }

    ///<summary>
    ///添加一个gameObject绑定
    ///<param name="typeId">Entity的TypeId</param>
    ///<param name="key">挂载信息的key，其实就是dictionary的key，手动删除的时候要用</param>
    ///<param name="loop">是否循环播放，直到手动删除</param>
    ///</summary>
    public void AddBindEffect(int typeId, string key, bool loop){
        if (key != "" && bindGameObject.ContainsKey(key) == true) return;    //已经存在，加不成


        
        
        var parentEntity = transform.GetComponentInParent<Entity>();
        if(parentEntity==null || parentEntity.IsUnityNull())
            return;
        
        var effectGo = GameEntry.Entity.ShowEffectEntity(typeId, transform.TransformPoint(offset), transform.rotation);//直接放到目标位置，不然联机部分会经过两次位置同步
        
        
        GameEntry.Entity.AttachEntity(effectGo,parentEntity,transform);
        
            
        effectGo.transform.localPosition = this.offset;
        effectGo.transform.localRotation = Quaternion.identity;
           
        //因为对象池的原因，这里不能一直Add
        SightEffect se = effectGo.GetOrAddComponent<SightEffect>();
            
        float duration = se.duration * (loop == false ? 1 : -1);
        BindGameObjectInfo bindGameObjectInfo = new BindGameObjectInfo(
            effectGo, duration
        );
        if (key != ""){
            this.bindGameObject.Add(key, bindGameObjectInfo);
        }else{
            this.bindGameObject.Add(
                Time.frameCount * Random.Range(1.00f, 9.99f) + "_" + Random.Range(1,9999),
                bindGameObjectInfo
            );
        }
       
            
    }

    ///<summary>
    ///移除一个gameObject的绑定
    ///<param name="key">挂载信息的key，其实就是dictionary的key</param>
    ///</summary>
    public void RemoveBindEffect(string key){
        if (bindGameObject.ContainsKey(key) == false) return;
        if (bindGameObject[key].effectEntity){
            //Destroy(bindGameObject[key].gameObject);
            GameEntry.Entity.HideEntity(bindGameObject[key].effectEntity.GetComponent<Entity>());
        }
        bindGameObject.Remove(key);
    }
}

///<summary>
///被挂载的gameobject的记录
///</summary>
public class BindGameObjectInfo{
    ///<summary>
    ///gameObject的地址
    ///</summary>
    public Entity effectEntity;

    ///<summary>
    ///还有多少时间之后被销毁，单位：秒
    ///</summary>
    public float duration;

    ///<summary>
    ///有些是不能被销毁的，得外部控制销毁，所以永久存在
    ///</summary>
    public bool forever;

    ///<summary>
    ///<param name="effectEntity">要挂载的gameObject</param>
    ///<param name="duration">挂的时间，时间到了销毁，[Magic]如果<=0则代表永久</param>
    ///</summary>
    public BindGameObjectInfo(EffectEntity effectEntity, float duration){
        this.effectEntity = effectEntity;
        this.duration = Mathf.Abs(duration);
        this.forever = duration <= 0;
    }
}