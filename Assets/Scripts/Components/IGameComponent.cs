using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGameComponent : MonoBehaviour
{
   protected virtual void Awake()
   {
      GameEntry.RegisterComponent(this);
     
   }
}
