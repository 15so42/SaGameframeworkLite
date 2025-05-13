using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpFollowingUi : FollowingUi
{

    public Image hpFill;
    private ChaState chaState;


    public override void Init(int followingConfigId)
    {
        base.Init(followingConfigId);
        hpFill.fillAmount = 1;
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
