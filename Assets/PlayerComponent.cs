using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : IGameComponent
{
   private Player player;

   public Player Player
   {
      get => player;
      private set => player = value;
   }

   public Entity SpawnPlayer(Vector3 pos,Quaternion rot)
   {
      return GameEntry.Entity.ShowBattleEntity(0, pos, rot,new BattleEntityUserData(GameEntry.Const.CONST_Camp_Player));
   }
}
