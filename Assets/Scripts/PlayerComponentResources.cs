using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源类型枚举
/// </summary>
public enum PlayerResourceType
{
    Gold,
    Silver,
}

/// <summary>
/// 玩家资源管理组件，支持增减、设置和获取资源，并在资源变化时发送事件
/// </summary>
public class PlayerComponentResources : MonoBehaviour
{
    // 存储所有资源的字典
    private readonly Dictionary<PlayerResourceType, float> _resources = new Dictionary<PlayerResourceType, float>();

    private void Awake()
    {
        // 初始化所有资源类型，默认值为0
        foreach (PlayerResourceType type in Enum.GetValues(typeof(PlayerResourceType)))
        {
            _resources[type] = 0f;
        }
    }

    /// <summary>
    /// 增加指定类型的资源
    /// </summary>
    public void AddRes(PlayerResourceType type, float value)
    {
        if (value == 0f) return;

        float prev = GetRes(type);
        float next = prev + value;
        _resources[type] = next;

        // 发送资源变化事件
        var evt = new PlayerComponentResChangeEvent(type, prev, next);
        TypeEventSystem.Send(evt);
    }

    /// <summary>
    /// 设置指定类型的资源为目标值
    /// </summary>
    public void SetRes(PlayerResourceType type, float value)
    {
        float prev = GetRes(type);
        if (Mathf.Approximately(prev, value)) return;

        _resources[type] = value;

        // 发送资源变化事件
        var evt = new PlayerComponentResChangeEvent(type, prev, value);
        TypeEventSystem.Send(evt);
    }

    /// <summary>
    /// 获取指定类型的资源当前值
    /// </summary>
    public float GetRes(PlayerResourceType type)
    {
        if (_resources.TryGetValue(type, out var value))
            return value;
        return 0f;
    }
}
