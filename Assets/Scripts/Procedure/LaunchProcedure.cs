using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Timer = UnityTimer.Timer;


public class LaunchProcedure : Procedure
{
    
    public override void OnEnter()
    {
        //读取设置
        var masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.6f);
        var bgmVolume = PlayerPrefs.GetFloat("bgmVolume", 0.6f);
        var sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        var uiSfxVolume = PlayerPrefs.GetFloat("uiSfxVolume", 1f);
        

        GameEntry.Sound.SetMasterVolume(masterVolume);
        GameEntry.Sound.SetBgmVolume(bgmVolume);
        GameEntry.Sound.SetSfxVolume(sfxVolume);
        GameEntry.Sound.SetUiSfxVolume(uiSfxVolume);
        
        var targetFrameRate=PlayerPrefs.GetInt("targetFrameRate", Application.isMobilePlatform?GameEntry.Const.CONST_DEFAULTMOBILEFRAMERATE: GameEntry.Const.CONST_DEFAULTPCFRAMERATE);
        Application.targetFrameRate = targetFrameRate;
        
        
        Timer.Register(0.5f, () =>
        {
            GameEntry.GetGameComponent<ProcedureComponent>().ChangeScene("MainMenu", new MainMenuProcedure());
        });

    }

    public override void OnUpdate()
    {
      
    }

    public override void OnExit()
    {
        
    }
}
