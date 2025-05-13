using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GameEndUiFormUserData
{
    
}
public class GameEndUiForm : UiForm
{
    
    public Text winnerText;
    public Button mainMenuBtn;
    public Button exitGameBtn;

    private GameEndUiFormUserData gameEndUiFormUserData;
    public override void Init(UiDataRow uiDataRow, object userData = null)
    {
        base.Init(uiDataRow, userData);
        gameEndUiFormUserData=userData as GameEndUiFormUserData;
        
        
        winnerText.text = "游戏结束：";
        
        mainMenuBtn.onClick.RemoveAllListeners();
        mainMenuBtn.onClick.AddListener(GoToMainMenu);
        
        exitGameBtn.onClick.RemoveAllListeners();
        exitGameBtn.onClick.AddListener(Application.Quit);
    }

    void GoToMainMenu()
    {
        GameEntry.Entity.HideAllEntity();
        GameEntry.Ui.CloseAllUiForm();
        GameEntry.Sound.StopAllAudio();
        
        
        GameEntry.Procedure.ChangeScene("MainMenu",new MainMenuProcedure());
    }

    public override bool HandleEscEvent()
    {
        return false;
    }
}
