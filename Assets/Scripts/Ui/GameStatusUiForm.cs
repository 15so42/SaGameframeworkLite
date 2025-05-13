using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusUiForm : UiForm
{
    public Text timerText;

    public override void OnUpdate()
    {
        base.OnUpdate();
        //显示游戏状态
    }

    public override bool HandleEscEvent()
    {
        return false;
    }
}
