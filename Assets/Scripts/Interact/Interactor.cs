using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    private List<Interaction> interactions = new List<Interaction>();

    public Action<Interaction> onInteractionChange;


    private Interactable currentInteractable;
    private void OnEnable()
    {
        onInteractionChange += OnInteractionChange;
    }

    void OnInteractionChange(Interaction interaction)
    {
        // 1. 检查 interaction 是否合法
        if (interaction == null || interaction.Equals(null))
        {
            // 取消当前焦点并清空
            if (currentInteractable != null)
            {
                currentInteractable.UnFocus();
            }
           
            currentInteractable = null;
            return;
        }

        // 2. 获取新的交互对象
        Interactable newInteractable = interaction.interactable;
        if (newInteractable == null)
        {
            if (currentInteractable != null)
            {
                currentInteractable.UnFocus();
            }
            
            currentInteractable = null;
            return;
        }

        // 3. 如果新旧对象不同，取消旧焦点
        if (currentInteractable != newInteractable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.UnFocus();
            }
            
            currentInteractable = newInteractable;
            currentInteractable.Focus();
        }
        // 4. 如果新旧对象相同，啥也不干
        else
        {
            // 可选：如果需重新触发 Focus 逻辑，可添加：
            // currentInteractable.Focus();
        }
    }
    private void OnDisable()
    {
        onInteractionChange -= OnInteractionChange;
    }

    public virtual void ReciveInteraction(Interaction interaction)
    {
        interactions.Add(interaction);
        
        onInteractionChange?.Invoke(interaction);
       
    }
    //
    // public virtual void UpdateInteraction(Interaction interaction)
    // {
    //     onInteractionChange?.Invoke(interaction);
    // }

    public virtual void Interact()
    {
        //交互最后一个添加进来的Interaction
        var last = GetLastInteraction();
        if (last == null)
        {
            onInteractionChange?.Invoke(null);
            return;
        }
           
        bool remove=last.interactable.Interact(this);
        if(remove)
            RemoveInteraction(last);
        else
        {
            last.text = last.interactable.interactStrategy.GetInteractionText();
            onInteractionChange?.Invoke(last);//更新文本
        }
    }

    protected Interaction GetLastInteraction()
    {
        if (interactions.Count > 0)
        {
            return interactions[interactions.Count - 1];
        }

        return null;
    }


    public virtual void RemoveInteraction(Interaction interaction)
    {
        interactions.Remove(interaction);
        
        
        var lastInteraction = GetLastInteraction();
        if (lastInteraction!=null)
        {
            
            onInteractionChange?.Invoke(lastInteraction);
        }
        else
        {
            onInteractionChange?.Invoke(null);
        }
    }
    
}
