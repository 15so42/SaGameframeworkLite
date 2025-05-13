using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHpFollowingUi : FollowingUi
{
    public Image hpFill;
    private ChaState chaState;
    private Player player;

    public Text playerNameText;

    public override void Init(int followingConfigId)
    {
        base.Init(followingConfigId);
        hpFill.fillAmount = 1;
       
    }

    void UpdatePlayerName(string playerName)
    {
        this.playerNameText.text = playerName;
    }
    public override void UpdateColor(Color color)
    {
        hpFill.color = color;
    }

    public override void Bind(GameObject go, string bindPointKey)
    {
        base.Bind(go, bindPointKey);
        chaState =go.GetComponent<ChaState>();

        chaState.onResourceChange += Refresh;

        player = go.GetComponent<Player>();
        
       
    }

    void Refresh()
    {
        hpFill.fillAmount = (float)chaState.resource.hp / chaState.property.hp;
    }

    private void OnDisable()
    {
        chaState.onResourceChange -= Refresh;
       
    }
}
