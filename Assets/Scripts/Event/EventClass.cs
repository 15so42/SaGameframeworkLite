using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventClass //因为类名要和文件匹配所以不得不使用这个类,
{
   
}



public class LoadingProgressEvent
{
    public float progress;

    public LoadingProgressEvent(float progress)
    {
        this.progress = progress;
    }
}

/// <summary>
/// 资源变化事件，用于通知资源数量变动
/// </summary>
public class PlayerComponentResChangeEvent
{
    public PlayerResourceType ResKey { get; }
    public float PrevValue { get; }
    public float NewValue { get; }

    public PlayerComponentResChangeEvent(PlayerResourceType resKey, float prevValue, float newValue)
    {
        ResKey = resKey;
        PrevValue = prevValue;
        NewValue = newValue;
    }
}

public class DoDamageEvent
{
    public DamageInfo damageInfo;
    public int dValue;
    public bool isHeal;

    public DoDamageEvent(DamageInfo damageInfo, int dValue, bool isHeal)
    {
        this.damageInfo = damageInfo;
        this.dValue = dValue;
        this.isHeal = isHeal;
    }
}

public class GameStartEvent
{
    public GameMode gameMode;

    public GameStartEvent(GameMode gameMode)
    {
        this.gameMode = gameMode;
    }
}


public class EntityShowEvent
{
    public Entity entity;

    public EntityShowEvent(Entity entity)
    {
        this.entity = entity;
    }
}


public class InputLockChangeEvent
{
    public int count;

    public InputLockChangeEvent(int count)
    {
        this.count = count;
    }
}

public class GameEndEvent
{
    public GameMode gameMode;

    public GameEndEvent(GameMode gameMode)
    {
        this.gameMode = gameMode;
    }
}