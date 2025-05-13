using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

public class Interaction
{
    public string text;
    public Interactable interactable;

    public Interaction(string text, Interactable interactable)
    {
        this.text = text;
        this.interactable = interactable;
    }
}
public abstract class Interactable : MonoBehaviour
{
    private HashSet<Collider> interactors=new HashSet<Collider>();

   
    private Interaction interaction;

    public Interaction GetInteraction()
    {
        return interaction;
    }

    public InteractStrategy interactStrategy;
    protected virtual void Awake()
    {
        interaction = new Interaction("交互", this);
        interactStrategy = Instantiate(interactStrategy);
        interactStrategy.Init(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(interactors.Contains(other))
            return;
        
        var interactor = other.GetComponent<Interactor>();
        if (interactor)
        {
            if (AddAble(interactor))
            {
                interaction.text = interactStrategy.GetInteractionText();
                interactor.ReciveInteraction(interaction);
                interactors.Add(other);
            }
        }
    }

    public HashSet<Collider> GetInteractors()
    {
        return interactors;
    }

    public virtual bool AddAble(Interactor interactor)
    {
        return interactStrategy.AddAble(interactor);
    }
   

    public virtual void Focus()
    {
        
    }
    
    public virtual void UnFocus()
    {
        
    }

  

    public virtual bool Interact(Interactor interactor)
    {
       return interactStrategy.BeInteract(interactor);
       
    }
    


    private void OnTriggerExit(Collider other)
    {
        if (interactors.Contains(other))
        {
            var interactor = other.GetComponent<Interactor>();
            if (interactor)
            {
                interactor.RemoveInteraction(interaction);
                interactors.Remove(other);
            }
        }
    }
}
