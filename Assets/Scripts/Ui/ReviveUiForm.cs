using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveUiFormUserData
{
    public int duration;

    public ReviveUiFormUserData(int duration)
    {
        this.duration = duration;
    }
}
public class ReviveUiForm : UiForm
{

    private ReviveUiFormUserData reviveUiFormUserData;
    public Text countDownText;

    public override void Init(UiDataRow uiDataRow, object userData = null)
    {
        base.Init(uiDataRow, userData);
        this.reviveUiFormUserData=userData as ReviveUiFormUserData;

        StartCoroutine(CountDown(reviveUiFormUserData.duration));
    }

    IEnumerator CountDown(int duration)
    {
        while (duration > 0)
        {
            countDownText.text = duration + "秒后修复完毕";
            yield return new WaitForSecondsRealtime(1);
            duration--;
        }
        Close();
    }

    public override bool HandleEscEvent()
    {
        return true;//拦截操作
    }
}
