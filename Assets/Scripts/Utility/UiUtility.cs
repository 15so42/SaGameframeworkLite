using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUtility : MonoBehaviour
{
    
    public static void ShowPlayerUiForm()
    {
        GameEntry.Ui.ShowUiForm(3);
    }
    public static void ShowTipUiForm(string msg)
    {
        GameEntry.Ui.ShowUiForm(4, new TipUiFormUserData(msg));
    }
    
    public static void ShowPopUpDialog(string title,string msg,string confirmBtnText,Action confirmAction,string cancelBtnText="",Action cancelAction=null,string thirdBtnText="",Action thirdAction=null,int level=0)
    {
        Color titleColor=new Color(63/255f,144/255f,155/255f);
        switch (level)
        {
            case 1:
                titleColor = Color.yellow;
                break;
            case 2:titleColor=Color.red;
                break;
                
        }
        GameEntry.Ui.ShowUiForm(7, new PopUpDialogUiFormUserData(title,titleColor,msg,confirmBtnText,confirmAction,cancelBtnText,cancelAction,thirdBtnText,thirdAction));
    }

    public static void ShowReviveUiForm(int duration)
    {
        GameEntry.Ui.ShowUiForm(8, new ReviveUiFormUserData(duration));
    }
    
    public static UiForm ShowGameStatusUiForm()
    {
        return GameEntry.Ui.ShowUiForm(9);
    }

    public static UiForm ShowSettingsUiForm()
    {
        return GameEntry.Ui.ShowUiForm(6);
    }
    
    public static UiForm ShowGameEndUiForm() 
    {
        return GameEntry.Ui.ShowUiForm(10,new GameEndUiFormUserData());
    }
    
}