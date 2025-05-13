using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class InteractAbleBuilding : Interactable
{
    private Outlinable outlinable;

    
    protected override void Awake()
    {
        base.Awake();
        outlinable = GetComponentInParent<Outlinable>();
        outlinable.enabled = false;
    }

    public override void Focus()
    {
        base.Focus();
        outlinable.enabled = true;
    }

    public override void UnFocus()
    {
        base.UnFocus();
        outlinable.enabled = false;
    }
}
