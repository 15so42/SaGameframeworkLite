using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureUtility : MonoBehaviour
{
    public static void GoToMainMenu()
    {
        GameEntry.Entity.HideAllEntity();
        GameEntry.Ui.CloseAllUiForm();
        GameEntry.Sound.StopAllAudio();
        GameEntry.GetGameComponent<ProcedureComponent>().ChangeScene("MainMenu",new MainMenuProcedure());
    }
}
