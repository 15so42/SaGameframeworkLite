using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingUiForm : UiForm
{
    
    //这个类没啥要干的，只是把所有FollowingUi放到这下面并且参与Ui组排序

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnClose()
    {
       base.OnClose();
    }

    public override bool HandleEscEvent()
    {
        return false;
    }
}
