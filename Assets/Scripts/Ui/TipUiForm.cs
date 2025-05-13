using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TipUiFormUserData
{
    public string message;

    public TipUiFormUserData(string message)
    {
        this.message = message;
    }
}
public class TipUiForm : UiForm
{
  
    public Transform tipTransform;
    public Text textComp;
    
    
    public override void Init(UiDataRow uiDataRow, object userData)
    {
        base.Init(uiDataRow,userData);

        var tipFormUserData = userData as TipUiFormUserData;

        textComp.text = tipFormUserData.message;

        tipTransform.localPosition = Vector3.zero;

       
        tipTransform.DOLocalMove( new Vector3(0, 100, 0), 2f).SetEase(Ease.InCubic).OnComplete(Close);
    }


    public override bool HandleEscEvent()
    {
        return false;
    }
}
