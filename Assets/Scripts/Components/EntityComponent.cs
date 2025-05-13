using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


public class EntityComponent : IGameComponent
{

   public Transform root;//生成的所有Entity放在root下面
   public List<Entity> entities = new List<Entity>();
   
   public Dictionary<int, ObjectPool<GameObject>> entityPools = new Dictionary<int, ObjectPool<GameObject>>();

   [Header("生成后是否归纳到root,联机模块不能归纳，否则一直报错")]
   public bool collectToRoot;

   public int seralId = 0;
   private void Update()
   {
      foreach (var entity in entities)//不会存在已经hide的Entity
      {
         entity.OnUpdate();
      }
   }
   
   private T ShowEntity<T>(int id, Vector3 pos, Quaternion rotation, SoDataRow concreteDataRow,object userData,Action<GameObject> onBeforeEnable) where T : Entity 
   {
      var dataRow = GameEntry.GetGameComponent<SoDataTableComponent>().GetSoDataRow<EntityDataRow>(id);
    
      GameObject targetGo = null;

      if (dataRow.usePool)
      {
         // 使用数据表ID作为对象池键
         if (entityPools.TryGetValue(dataRow.id, out var targetPool))
         {
            targetGo = targetPool.Get();
         }
         else
         {
            // 创建新的对象池并缓存
            targetPool = new ObjectPool<GameObject>(
               createFunc: () => 
               {
                  var go = GameObject.Instantiate(dataRow.pfb,pos,rotation);
                  go.SetActive(false); // 初始状态设为非激活
                  return go;
               }
            );

            entityPools.Add(dataRow.id, targetPool);
            targetGo = targetPool.Get();
         }
      }
      else
      {
         // 非池化直接实例化
         targetGo = GameObject.Instantiate(dataRow.pfb,pos,rotation);
      }
      
      
      targetGo.SetActive(false);
      targetGo.transform.position = pos;
      targetGo.transform.rotation = rotation;
      
      
      onBeforeEnable?.Invoke(targetGo);
      
      targetGo.SetActive(true);
      
      
      if(collectToRoot)
         targetGo.transform.SetParent(root);
      // 获取或添加Entity组件
      var entityComp = targetGo.GetComponent<T>() ?? targetGo.AddComponent<T>();
      
      seralId++;
      entityComp.serialId = seralId;
    
      // 使用原型模式避免数据引用问题
      var clonedData = dataRow.Clone() as EntityDataRow;//使用 CloneViaSerialization(forceReflected: true) 回退到纯反射实现,不然代码被剥离无法正常工作
      
     
      
      entityComp.Init(clonedData,concreteDataRow, userData);
     
      entityComp.OnShow();
      
      entities.Add(entityComp);
      
      //事件播报
      TypeEventSystem.Send(new EntityShowEvent(entityComp));
      return entityComp;
   }

   public T GetEntityBySeralId<T>(int targetId) where T:Entity
   {
      return entities.Find(x => x.serialId == targetId) as T;
   }
   
   public ModelEntity ShowModelEntity(int entityId, Vector3 pos,Quaternion rotation,object userData=null,Action<GameObject> onBeforeInit=null)
   {
      return ShowEntity<ModelEntity>(entityId, pos, rotation, null,userData,onBeforeInit);
   }
   
   public BattleEntity ShowBattleEntity(int battleEntityId, Vector3 pos,Quaternion rotation,object userData=null,Action<GameObject> onBeforeInit=null)
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<BattleEntityDataRow>(battleEntityId);
      return ShowEntity<BattleEntity>(dataRow.entityId, pos, rotation, dataRow,userData,onBeforeInit);
   }
   
   public EffectEntity ShowEffectEntity(int effectEntityId, Vector3 pos,Quaternion rotation,object userData=null,Action<GameObject> onBeforeInit=null)
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<EffectEntityDataRow>(effectEntityId);
      return ShowEntity<EffectEntity>(dataRow.entityId, pos, rotation, dataRow,userData,onBeforeInit);
   }
   
   public WeaponEntity ShowWeaponEntity(int weaponEntityId,Entity parentEntity,Transform parentTransform,object userData=null,Action<GameObject> onBeforeInit=null)
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<WeaponEntityDataRow>(weaponEntityId);
      var weaponEntity=ShowEntity<WeaponEntity>(dataRow.entityId, Vector3.zero, Quaternion.identity, dataRow,userData,onBeforeInit);
      GameEntry.Entity.AttachEntity(weaponEntity,parentEntity,parentTransform);
      return weaponEntity;
   }
   
   public ArmorEntity ShowArmorEntity(int armorEntityId,Entity parentEntity,Transform parentTransform,object userData=null,Action<GameObject> onBeforeInit=null)
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<ArmorEntityDataRow>(armorEntityId);
      var armorEntity=ShowEntity<ArmorEntity>(dataRow.entityId, Vector3.zero, Quaternion.identity, dataRow,userData,onBeforeInit);
      GameEntry.Entity.AttachEntity(armorEntity,parentEntity,parentTransform);
      return armorEntity;
   }

   public CameraEntity ShowCameraEntity(int cameraEntityId, Vector3 pos, Quaternion rotation, object userData=null,Action<GameObject> onBeforeInit=null)
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<CameraEntityDataRow>(cameraEntityId);
      return ShowEntity<CameraEntity>(dataRow.entityId, pos, rotation, dataRow,userData,onBeforeInit);
   }
   
   
   
   
   //Bullet和Aoe和上面的实体不同，Bulle和Aoe的配置是在DesignerTable中通过脚本配置的而不是使用SoDataTable配置的，所以直接生成entity,不需要Bullet和Aoee表
   public BulletEntity ShowBulletEntity(int bulletEntityId, Vector3 pos, object userData=null,Action<GameObject> onBeforeInit=null)//Bullet的旋转是在bulletLauncher中计算的
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<BulletEntityDataRow>(bulletEntityId);
      return ShowEntity<BulletEntity>(dataRow.entityId, pos, Quaternion.identity, dataRow,userData,onBeforeInit);
     
   }
   
   public AoeEntity ShowAoeEntity(int aoeEntityId,  object userData=null,Action<GameObject> onBeforeInit=null)//Aoe的位置和旋转是在AoeLauncher中计算的
   {
      var dataRow = GameEntry.SoDataTable.GetSoDataRow<AoeEntityDataRow>(aoeEntityId);
      return ShowEntity<AoeEntity>(dataRow.entityId, Vector3.zero, Quaternion.identity, dataRow,userData,onBeforeInit);
   }
   
   


   public void HideEntity(Entity entity)
   {
      
      entities.Remove(entity);
      entity.OnHide();
     
      
      if(entity.IsUnityNull())//联机的时候关闭服务端时所有物体被Despawn，也就是被销毁了，entity还持有，但实际上已经是null了.
         return;
      
      if (entityPools.ContainsKey(entity.entityDataRow.id))
      {
         entityPools[entity.entityDataRow.id].Release(entity.gameObject);
      }
      
      entity.gameObject.SetActive(false);
      
   }

   public void HideAllEntity()
   {
      for (int i = 0; i < entities.Count; i++)
      {
         HideEntity(entities[i]);
         i--;
      }
   }


   public void AttachEntity(Entity childEntity,Entity targetEntity,Transform parent)
   {
      if (childEntity.parentEntity != null)
      {
         childEntity.parentEntity.OnDetached(childEntity);
         childEntity.OnDetachFrom(childEntity.parentEntity);
      }

      childEntity.transform.SetParent(parent);
      childEntity.transform.localPosition=Vector3.zero;
      childEntity.transform.localRotation=Quaternion.identity;
      
      childEntity.OnAttachTo(targetEntity);
      targetEntity.OnAttached(childEntity);
   }
   public void DetachEntity(Entity entity)
   {
      if(entity.parentEntity==null)
         return;
      
      
      entity.parentEntity.OnDetached(entity);
      entity.OnDetachFrom(entity.parentEntity);
      if(collectToRoot)
         entity.transform.SetParent(root);
      else
      {
         entity.transform.SetParent(null);
      }
      
      
   }
  
}
