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
            SceneController.Instance.StopTime();
        }
        else
        {
            SceneController.Instance.StartTime();
        }
    }

    public void LoadMainMenu()
    {
        SceneController.Instance.StartTime();
        SceneController.Instance.LoadMainMenu();
    }

    public void RestartLevel()
    {
        SceneController.Instance.StartTime();
        SceneController.Instance.LoadCurrentScene();
    }
}
