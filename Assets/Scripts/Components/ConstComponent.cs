using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ConstComponent : IGameComponent
{
    [NaughtyAttributes.ReadOnly][Header("设置为空字符串和string默认值匹配")]
    public string CONST_Camp_Unknown = "";
    public string CONST_Camp_Monster = "Monster";
    public string CONST_Camp_Player = "Player";
    
    
    [Header("绑点")]
    public string CONST_BPKey_WeaponPoint = "Weapon";
    public string CONST_BPKey_ArmorPoint = "Armor";
    
    [Header("最大游戏时间")]
    public int CONST_Game_Seconds = 480;
    
    
    
    [Header("帧率设置")]
    public int CONST_DEFAULTMOBILEFRAMERATE = 45;
    public int CONST_DEFAULTPCFRAMERATE = 90;
    public int CONST_MINFRAMERATE = 30;
    public int CONST_MAXMOBILEFRAMERATE = 60;
    public int CONST_MAXPCFRAMERATE = 120;
    
    [Header("玩家出生点标签")]
    public string CONST_PLAYERSPAWNPOSTAG= "PlayerSpawnPos";
    
}
