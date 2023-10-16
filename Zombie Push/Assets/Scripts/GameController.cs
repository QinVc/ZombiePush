using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public enum GameStage
{
    None,
    FindMsg,
    Escap,
    Win,
    Lose
}

public class GameController : MonoBehaviour
{
    public GameObject startUI;
    public GameObject gameUI;
    public GameObject pauseUI;
    public GameObject loseUI;

    public GameObject[] EscapePoints;
    public GameObject[] EscapMsg;
    public Transform EscapePoint;
    public int HavedEscapsMsgCount = 0;
    GameStage Stage = GameStage.None;

    // Start is called before the first frame update
    void Start()
    {
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
        loseUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        World.GameWorld.GetComponent<DelegateManager>().OnGameStageChange+=GameStageChange;
        World.GameWorld.GetComponent<DelegateManager>().OnPlayerGetMsg+=OnPlayerGetEscapeMsg;

        WaitForStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !loseUI.activeSelf)
            Pausing();       
    }

    public void GameStageChange(GameStage stage)
    {
        if (stage == GameStage.FindMsg) 
        {
            StartGame();
        }
        else if (stage == GameStage.Escap) 
        {
            float MaxDistance = 0;
            int MaxIndex = 0;
            for(int i = 0; i < EscapePoints.Length; i++) 
            {
                float Distance = Vector3.Magnitude(EscapePoints[i].transform.position-World.GameWorld.GetComponent<World>().player.position);
                if (Distance > MaxDistance) 
                {
                    MaxDistance = Distance;
                    MaxIndex = i;
                }
            }

            EscapePoints[MaxIndex].SetActive(true);
            EscapePoint = EscapePoints[MaxIndex].transform;
        }
        else if(stage == GameStage.Win) 
        {
            Debug.Log("Win!");
        }

        else if (stage == GameStage.Lose)
        {
            PlayerLose();
        }
    }
    public void WaitForStart()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameUI.SetActive(false);
        startUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Pausing()
    {
        if (Time.deltaTime == 0f)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            gameUI.SetActive(true);
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameUI.SetActive(false);
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Restarting()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayerLose()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        loseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    void OnPlayerGetEscapeMsg() 
    {
        HavedEscapsMsgCount++;

        if (HavedEscapsMsgCount == EscapMsg.Length) 
        {
            GameStageChange(GameStage.Escap);
        }
    }
}
