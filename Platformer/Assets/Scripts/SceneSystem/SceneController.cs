using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private int menuIndex;
    [SerializeField]
    private int startLevelIndex;

    public static SceneController Instance { get; private set; }

    private void Awake()
    {
#if UNITY_EDITOR
        var controllers = FindObjectsOfType<SceneController>();
        if (controllers.Count() > 1) Debug.LogWarning("Multiple scene controllers present in one scene.");
#endif
        Instance = this;
    }

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
        if (levelIndex >= startLevelIndex)
        {
            return levelIndex;
        }
        return -1;
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
