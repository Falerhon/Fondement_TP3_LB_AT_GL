using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

    public static bool GameIsPause = false;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject SettingUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPause = false;
        GameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPause = true;
        GameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnSetting()
    {
        SettingUI.SetActive(true);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
