using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    public void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadMainMenu();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadCurrentScene();
    }
}
