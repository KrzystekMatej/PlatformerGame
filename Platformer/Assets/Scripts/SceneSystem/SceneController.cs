using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : GlobalComponent<SceneController>
{
    [SerializeField]
    private int menuIndex;
    [SerializeField]
    private int startLevelIndex;

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(index);
        else SceneManager.LoadScene(menuIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(menuIndex);
    }

    public void LoadLevelByIndex(int levelIndex)
    {
        SceneManager.LoadScene(startLevelIndex + levelIndex);
    }

    public int GetCurrentLevelIndex()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex - startLevelIndex;
        if (levelIndex >= 0)
        {
            return levelIndex;
        }
        return -1;
    }
}
