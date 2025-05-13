using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BattleProcedure : Procedure
{
    public GameMode GameMode
    {
        get;
        set;
    }

    private bool gameOverChecked;

    public BattleProcedure(GameMode gameMode)
    {
        this.GameMode = gameMode;
    }


    public override void OnEnter()
    {
        gameOverChecked = false;
        
        GameMode.OnEnter();
        
        //Bgm可以配表
        GameEntry.Sound.PlayBGM(0);
        
     
    }

    public override void OnUpdate()
    {
        GameMode.OnUpdate();

        if (!gameOverChecked && GameMode.IsGameOver())
        {
            gameOverChecked = true;

            
            TypeEventSystem.Send(new GameEndEvent(GameMode));
            
        }
    }

 

  


    public override void OnExit()
    {
       GameMode.OnExit();
    }
}
