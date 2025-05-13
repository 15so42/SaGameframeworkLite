using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class MainMenuUiForm : UiForm
{

    public Button startButton;
    public Button settingsButton;


    public override void OnOpen()
    {
        base.OnOpen();
        
        
        startButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        
        startButton.onClick.AddListener(() =>
        {
            GameEntry.Procedure.ChangeScene("Battle",new BattleProcedure(new PVPGameMode()));
        });
        
        settingsButton.onClick.AddListener(()=>
        {
            UiUtility.ShowSettingsUiForm();
        });
    }


    public override bool HandleEscEvent()
    {
        return false;
    }
}
