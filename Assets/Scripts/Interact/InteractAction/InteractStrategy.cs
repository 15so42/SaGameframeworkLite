using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


public abstract class InteractStrategy : ScriptableObject
{
    private int interactedCount;
    public int InteractedCount
    {
        get => interactedCount;
        set
        {
            interactedCount = value;
            onInteractCountChange?.Invoke(interactedCount);
        }
    }

    public Action<int> onInteractCountChange;

    public virtual string GetInteractionText()
    {
        return "交互";
    }

    [ReadOnly]
    public Interactable owner;//属主
    public virtual void Init(Interactable interactable)
    {
        this.owner = interactable;
    }
    public abstract bool AddAble(Interactor interactor);
    public abstract bool BeInteract(Interactor interactor);
}
