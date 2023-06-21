using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private int startLevelIndex, menuIndex;

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadStartLevel()
    {
        SceneManager.LoadScene(startLevelIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(GetNextLevelIndex());
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevelByIndex(int levelIndex)
    {
        SceneManager.LoadScene(startLevelIndex + levelIndex - 1);
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(menuIndex);
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
    }

    public int GetNextLevelIndex()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index < SceneManager.sceneCountInBuildSettings)
        {
            return index;
        }
        else
        {
            return menuIndex;
        }

    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
    }
}
