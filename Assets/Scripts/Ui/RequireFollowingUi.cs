using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireFollowingUi : MonoBehaviour
{

    public int targetFollowingUiId;
    [HideInInspector]public FollowingUi followingUi;

    public void SetFollowingUi(FollowingUi followingUi)
    {
        this.followingUi = followingUi;
    }
    private void OnDisable()
    {
        if (followingUi)
            followingUi.Release();
    }
}
