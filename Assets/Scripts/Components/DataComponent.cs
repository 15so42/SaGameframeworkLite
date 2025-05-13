using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class DataComponent : IGameComponent
{
   public Dictionary<string, object> datas = new Dictionary<string, object>();

   public T GetData<T>(string key,object defaultValue)
   {
      if (datas.TryGetValue(key, out var value))
      {
         return (T)value;
      }

      return (T)defaultValue;
   }

   public void SetData(string key,object value)
   {
      if (datas.ContainsKey(key))
      {
         datas[key] = value;
      }
      else
      {
         datas.Add(key,value);
      }
   }
}
