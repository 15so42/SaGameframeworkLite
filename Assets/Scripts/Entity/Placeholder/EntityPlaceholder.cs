using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityPlaceholder : MonoBehaviour
{

    public int id;


    private void Start()
    {
        ShowEntity();
    }

    public abstract void ShowEntity();

   
}
