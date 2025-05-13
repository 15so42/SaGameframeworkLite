using System;
using System.Collections.Generic;

public class TypeEventSystem
{
    /// <summary>
    /// 接口 只负责存储在字典中，使用这个接口是因为字典里没法直接声明Action加泛型的形式,所以使用这个来间接存储
    /// </summary>
    interface IRegisterations
    {
    }

    /// <summary>
    /// 多个注册
    /// </summary>
    class Registerations<T> : IRegisterations
    {
        /// <summary>
        /// 委托本身可以一对多注册
        /// </summary>
        public Action<T> onReceives;
    }

    /// <summary>
    /// 
    /// </summary>
    private static Dictionary<Type, IRegisterations> mTypeEventDict = new Dictionary<Type, IRegisterations>();

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="onReceive"></param>
    /// <typeparam name="T"></typeparam>
    public static void Register<T>(Action<T> onReceive)
    {
        var type = typeof(T);

        IRegisterations registerations = null;

        if (mTypeEventDict.TryGetValue(type, out registerations))
        {
            var reg = registerations as Registerations<T>;
            reg.onReceives += onReceive;
        }
        else
        {
            var reg = new Registerations<T>();
            reg.onReceives += onReceive;
            mTypeEventDict.Add(type, reg);
        }
    }

    /// <summary>
    /// 注销事件
    /// </summary>
    /// <param name="onReceive"></param>
    /// <typeparam name="T"></typeparam>
    public static void UnRegister<T>(Action<T> onReceive)
    {
        var type = typeof(T);

        IRegisterations registerations = null;

        if (mTypeEventDict.TryGetValue(type, out registerations))
        {
            var reg = registerations as Registerations<T>;
            reg.onReceives -= onReceive;
        }
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="t"></param>
    /// <typeparam name="T"></typeparam>
    public static void Send<T>(T t)
    {
        var type = typeof(T);
        IRegisterations registerations = null;
        if (mTypeEventDict.TryGetValue(type, out registerations))
        {
            var reg = registerations as Registerations<T>;
            if(reg!=null)
                reg.onReceives(t);
        }
    }
}