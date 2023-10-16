using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        World.GameWorld.GetComponent<DelegateManager>().OnGameStageChange(GameStage.Win);
    }
}
