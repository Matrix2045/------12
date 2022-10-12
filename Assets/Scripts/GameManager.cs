using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Platform { 
   None,
   Web
}
public class GameManager :MonoBehaviour
{
   private static GameManager  Instance;
   public static  GameManager GetInstance
   {
        get {
            if (Instance == null) {
                Instance = new GameManager();
            }
            return Instance;
        }
   }
    public Platform platform=Platform.None;
}
