using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    private SceneController sceneController;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
    }


    public void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            sceneController.StopTime();
        }
        else
        {
            sceneController.StartTime();
        }
    }

    public void LoadMainMenu()
    {
        sceneController.StartTime();
        sceneController.LoadMainMenu();
    }

    public void RestartLevel()
    {
        sceneController.StartTime();
        sceneController.LoadCurrentScene();
    }
}
