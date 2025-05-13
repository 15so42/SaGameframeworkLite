using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUiForm : UiForm
{
    private ChaState chaState;
    private PlayerComponentResources playerComponentResources;
    private PlayerInteractor playerInteractor;
    private Player player;

    [Header("ChaState状态")]
    public Image hpFill;

    [Header("玩家资源")]
    public Text goldText;
    public Text silverText;

    [Header("交互提示")] 
    [SerializeField]private GameObject interactionTip;
    [SerializeField]private Text interactionText;

   

      [Header("安卓控制")]
      public GameObject moveStick;
      public GameObject rightStick;
     
      public OnScreenButton interactScreenButton;

      private bool isAndroid;
      

      
      public override void Init(UiDataRow uiDataRow, object userData)
    {
        base.Init(uiDataRow,userData);
        interactionTip.gameObject.SetActive(false);
        
       

        isAndroid = Application.isMobilePlatform;
        if (isAndroid)
        {
            moveStick.gameObject.SetActive(true);
            rightStick.gameObject.SetActive(true);

            
            interactScreenButton.enabled = true;
        }
        else
        {
            moveStick.gameObject.SetActive(false);
            rightStick.gameObject.SetActive(false);

           
            interactScreenButton.enabled = false;
        }

        hpFill.fillAmount = 1;
      

    }

    public void BindChaState(ChaState chaState)
    {
        this.chaState = chaState;
        chaState.onResourceChange += RefreshHp;
    }

    public void BindResource(PlayerComponentResources playerComponentResources)
    {
        this.playerComponentResources = playerComponentResources;
        
        GameEntry.RegisterEvent<PlayerComponentResChangeEvent>(RefreshResource);
    }

    public void BindInteractor(PlayerInteractor interactor)
    {
        this.playerInteractor = interactor;

        this.playerInteractor.onInteractionChange += RefreshInteractionButton;

    }

    public void BindPlayer(Player player)
    {
        this.player = player;
    }

   

    void RefreshHp()
    {
        hpFill.fillAmount = (float)chaState.resource.hp / chaState.property.hp;
    }

    void RefreshResource(PlayerComponentResChangeEvent changeEvent)
    {
        
    }

    void RefreshInteractionButton(Interaction interaction)
    {
        if (interaction == null)
        {
            interactionTip.gameObject.SetActive(false);
        }
        else
        {
            
            interactionTip.gameObject.SetActive(true);
            interactionText.text = interaction.text;
        }
    }

  
    
    
    

    public void ShowInteract(string interactionText)
    {
        interactionTip.gameObject.SetActive(true);
        this.interactionText.text = interactionText;
    }

    public void CloseInteract()
    {
        interactionTip.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
       
    }

    public override bool HandleEscEvent()
    {
        return false;
    }

    public override void OnClose()
    {
        base.OnClose();
        chaState.onResourceChange -= RefreshHp;//都是ResourceChange,但是chastate的资源是指血量等资源。playerResources值得是金币等资源
        GameEntry.UnRegisterEvent<PlayerComponentResChangeEvent>(RefreshResource);
        this.playerInteractor.onInteractionChange -= RefreshInteractionButton;
      
    }
}
