using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseUI;
    public GameObject loseUI;

    // Start is called before the first frame update
    void Start()
    {
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
        loseUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !loseUI.activeSelf)
            Pausing();       
    }

    //пауза игры
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

    //перезагрузка игры
    public void Restarting()
    {
        SceneManager.LoadScene(0);
    }

    //обработчик проигрыша игрока
    public void PlayerLose()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        loseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    //выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }
}
