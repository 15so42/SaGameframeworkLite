using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class FlyTextComponent : IGameComponent
{
    private FlyTextUiForm flyTextUiForm;

    public int textSize=64;
    public float worldJumpRadius = 1;
    public float worldJumpHeight = 1;
    public float jumpDuration = 2;
    public Ease ease = Ease.Linear;

    public Action<Vector3, string> onFlyTextAction;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        TypeEventSystem.Register<GameStartEvent>(OnGameStart);
    }

    private void OnDisable()
    {
        TypeEventSystem.UnRegister<GameStartEvent>(OnGameStart);
    }

    
    
   
    public void FlyText(Vector3 worldPos,string msg)
    {

        if (flyTextUiForm==null ||flyTextUiForm.closed)//因为切换场景时所有UiForm会被关闭
        {
            NewFlyTextUiForm();
        }
        flyTextUiForm.FlyText(worldPos, msg );
        onFlyTextAction?.Invoke(worldPos,msg);
    }

    public void OnGameStart(GameStartEvent gameStartEvent)
    {
        
    }

    void NewFlyTextUiForm()
    {
        
        flyTextUiForm = GameEntry.Ui.ShowUiForm(1) as FlyTextUiForm;
        
        flyTextUiForm.textSize = textSize;
        flyTextUiForm.jumpDuration = jumpDuration;
        flyTextUiForm.worldJumpRadius = worldJumpRadius;
        flyTextUiForm.worldJumpHeight = worldJumpHeight;
        flyTextUiForm.ease = ease;
    }
    
    
}
