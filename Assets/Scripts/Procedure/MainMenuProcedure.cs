using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuProcedure : Procedure
{
    
    public override void OnEnter()
    {
        GameEntry.GetGameComponent<UiComponent>().ShowUiForm(0, null);
        
        GameEntry.Sound.PlayBGM(1);
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
       
    }
}
