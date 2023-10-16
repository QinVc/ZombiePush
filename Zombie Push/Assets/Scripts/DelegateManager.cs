using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FOnPlayerBeDamge(int value);
public delegate void FOnGameStageChange(GameStage stage);
public delegate void FOnPlayerGetMsg();
public class DelegateManager : MonoBehaviour
{
    public FOnPlayerBeDamge OnPlayerBeDamge;
    public FOnGameStageChange OnGameStageChange;
    public FOnPlayerGetMsg OnPlayerGetMsg;
}
