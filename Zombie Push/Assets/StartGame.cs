using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void OnClickStart() 
    {
        World.GameWorld.GetComponent<DelegateManager>().OnGameStageChange(GameStage.FindMsg);
    }
}
