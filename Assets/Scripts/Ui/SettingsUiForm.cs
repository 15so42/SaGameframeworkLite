using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsUiForm : UiForm
{

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider uiSfxSlider;
    
    
    public Slider frameRateSlider;

    public Button continueBtn;
    public Button instructionBtn;
    public Button mainMenuBtn;
    public Button exitButton;


    private const int minFrameRate = 30;
    private int maxFrameRate = 60;
    public Text maxFrameRateText;
    public override void Init(UiDataRow uiDataRow, object userData = null)
    {
        base.Init(uiDataRow, userData);

        masterSlider.value = GameEntry.Sound.masterVolume;
        bgmSlider.value = GameEntry.Sound.bgmVolume;
        sfxSlider.value = GameEntry.Sound.sfxVolume;
        uiSfxSlider.value = GameEntry.Sound.uiSfxVolume;


        maxFrameRate = Application.isMobilePlatform ? GameEntry.Const.CONST_MAXMOBILEFRAMERATE : GameEntry.Const.CONST_MAXPCFRAMERATE;
        maxFrameRateText.text = maxFrameRate.ToString();
        var rate = (float)(Application.targetFrameRate - GameEntry.Const.CONST_MINFRAMERATE) /
                   (maxFrameRate - GameEntry.Const.CONST_MINFRAMERATE);
        frameRateSlider.value = rate;
    }

    private void Start()
    {
        masterSlider.onValueChanged.AddListener((x)=>
        {
            GameEntry.Sound.SetMasterVolume(x);
            PlayerPrefs.SetFloat("masterVolume",x);
        });
        bgmSlider.onValueChanged.AddListener((x)=>
        {
            GameEntry.Sound.SetBgmVolume(x);
            PlayerPrefs.SetFloat("bgmVolume",x);
        });
        sfxSlider.onValueChanged.AddListener((x)=>
        {
            GameEntry.Sound.SetSfxVolume(x);
            PlayerPrefs.SetFloat("sfxVolume",x);
        });
        uiSfxSlider.onValueChanged.AddListener((x)=>
        {
            GameEntry.Sound.SetUiSfxVolume(x);
            PlayerPrefs.SetFloat("uiSfxVolume",x);
        });
        
        frameRateSlider.onValueChanged.AddListener((x) =>
        {
            var frameRate = Mathf.RoundToInt(Mathf.Lerp(minFrameRate, maxFrameRate, x));
            Application.targetFrameRate = frameRate;
            PlayerPrefs.SetInt("targetFrameRate",frameRate);
        });
        
        continueBtn.onClick.AddListener(() =>
        {
            Close();
        });   
        
        instructionBtn.onClick.AddListener(() =>
        {
            //开启玩法说明Uiform
        });
        
        mainMenuBtn.onClick.AddListener(() =>
        {
            //弹出确认窗口
        });
        
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public override bool HandleEscEvent()
    {
        Close();
        return true;
    }
}
