using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDialogUiFormUserData
{
    public string title;
    public Color titleColor=Color.cyan;
    public string msg;
    
    
    public string confirmBtnText;
    public Action confirmAction;
    
    public string cancelBtnText;
    public Action cancelAction;
    
    public string thirdBtnText;
    public Action thirdAction;

    public PopUpDialogUiFormUserData(string title,Color titleColor, string msg, string confirmBtnText, Action confirmAction, string cancelBtnText, Action cancelAction, string thirdBtnText, Action thirdAction)
    {
        this.title = title;
        this.titleColor = titleColor;
        this.msg = msg;
        this.confirmBtnText = confirmBtnText;
        this.confirmAction = confirmAction;
        this.cancelBtnText = cancelBtnText;
        this.cancelAction = cancelAction;
        this.thirdBtnText = thirdBtnText;
        this.thirdAction = thirdAction;
    }
}
public class PopUpDialogUiForm : UiForm
{
    private PopUpDialogUiFormUserData popUpDialogUiFormUserData;

    public Image titleBg;
    public Text titleText;
    public Text msgText;
    
    
  
    public Button confirmBtn;
    public Button cancelBtn;
    public Button thirdBtn;

    public Text confirmBtnText;
    public Text cancelBtnText;
    public Text thirdBtnText;
    
    
    

    public override void Init(UiDataRow uiDataRow, object userData = null)
    {
        base.Init(uiDataRow, userData);
        popUpDialogUiFormUserData=userData as PopUpDialogUiFormUserData;

        titleText.text = popUpDialogUiFormUserData.title;
        titleBg.color = popUpDialogUiFormUserData.titleColor;
        msgText.text = popUpDialogUiFormUserData.msg;
        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        thirdBtn.onClick.RemoveAllListeners();
        
        confirmBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        thirdBtn.gameObject.SetActive(false);

        confirmBtnText.text = popUpDialogUiFormUserData.confirmBtnText;
        cancelBtnText.text = popUpDialogUiFormUserData.cancelBtnText;
        thirdBtnText.text = popUpDialogUiFormUserData.thirdBtnText;

        if (String.IsNullOrEmpty(popUpDialogUiFormUserData.confirmBtnText)==false)
        {
            confirmBtn.gameObject.SetActive(true);
            confirmBtn.onClick.AddListener(()=>
            {
                popUpDialogUiFormUserData.confirmAction?.Invoke();
                Close();
            });
        }
        
        if (String.IsNullOrEmpty(popUpDialogUiFormUserData.cancelBtnText)==false)
        {
            cancelBtn.gameObject.SetActive(true);
            cancelBtn.onClick.AddListener(()=>
            {
                popUpDialogUiFormUserData.cancelAction?.Invoke();
                Close();
            });
        }
        
        if (String.IsNullOrEmpty(popUpDialogUiFormUserData.thirdBtnText)==false)
        {
            thirdBtn.gameObject.SetActive(true);
            thirdBtn.onClick.AddListener(()=>
            {
                popUpDialogUiFormUserData.thirdAction?.Invoke();
                Close();
            });
        }
        

    }

    public override bool HandleEscEvent()
    {
        if (popUpDialogUiFormUserData != null && !String.IsNullOrEmpty(popUpDialogUiFormUserData.cancelBtnText))
        {
            popUpDialogUiFormUserData.cancelAction?.Invoke();
            Close();
           
        }

        return true;//无论有没有cancelAction都要拦截Esc处理

    }
}
