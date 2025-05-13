using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

public class PVPGameMode : GameMode
{

    private bool gameEndChecked;//标志位，避免游戏结束后还检测游戏是否结束
    private bool isGameOver;
    private bool gameStarted;
    public override bool IsGameOver()
    {
        return isGameOver;
    }

    private double startTime;
    public override void OnEnter()//初始化写在这里
    {
        base.OnEnter();

        //初始化变量
        isGameOver = false;
        gameEndChecked = false;//有确保只执行一次GameOver
        gameStarted = false;
        
        
        List<string> camps=new List<string>();
        //阵营
        camps.Add(GameEntry.Const.CONST_Camp_Unknown);
        camps.Add(GameEntry.Const.CONST_Camp_Monster);
        camps.Add(GameEntry.Const.CONST_Camp_Player+"0");
        camps.Add(GameEntry.Const.CONST_Camp_Player+"1");
        camps.Add(GameEntry.Const.CONST_Camp_Player+"2");
        camps.Add(GameEntry.Const.CONST_Camp_Player+"3");


        
        for (int i = 0; i < camps.Count; i++)
        {
            GameEntry.Combat.RegisterCamp(camps[i]);
        }

        for (int i = 0; i < camps.Count; i++)
        {
            for (int j = 0; j < camps.Count; j++)
            {
                if (i != j)
                {
                    GameEntry.Combat.BeEnemies(camps[i],camps[j]);
                }
            }
        }

        
    }

   

    
    public void StartGame()
    {
        //生成玩家之类的
        var spawnPosTrans=GameObject.FindGameObjectWithTag(GameEntry.Const.CONST_PLAYERSPAWNPOSTAG).transform;
        Vector3 spawnPos=Vector3.zero;
        Quaternion spawnRot=Quaternion.identity;
        if (spawnPosTrans != null)
        {
            spawnPos=spawnPosTrans.position;
            spawnRot=spawnPosTrans.rotation;
        }
        var playerEntity=GameEntry.PlayerComponent.SpawnPlayer(spawnPos,spawnRot);
        
        //相机
        var cameraEntity = GameEntry.Entity.ShowCameraEntity(0, spawnPos, spawnRot);
        cameraEntity.SetTarget(playerEntity);
    }


   


    
    public override void OnUpdate()
    {
        if (!gameStarted )
        {
           
            startTime = Time.time;
            StartGame();
            gameStarted = true;
            

        }
        // 每帧检测
        if (!gameEndChecked &&
            (
                (Application.isEditor && Keyboard.current.kKey.isPressed)//K键强制结束游戏
                || Time.time - startTime >= GameEntry.Const.CONST_Game_Seconds//大于最大对局时间
            ))
        {
            GameOver();
        }
        
        
    }

    public void GameOver()//比如只剩房主时可以强行结束游戏
    {
        gameEndChecked = true;
        //游戏结束
        isGameOver = true;
        TypeEventSystem.Send(new GameEndEvent(this));
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
