using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode
{
   public abstract bool IsGameOver();//包含所有情形，输赢平。

   public virtual void OnEnter()
   {
      TypeEventSystem.Send(new GameStartEvent(this));
   }
   public abstract void OnUpdate();

   public virtual void OnExit()
   {
      
   }

}
