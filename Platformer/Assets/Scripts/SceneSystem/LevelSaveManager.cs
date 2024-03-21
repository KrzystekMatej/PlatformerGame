using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSaveManager
{
    private static string reachedLevelKey = "ReachedLevel";

    public static int GetReachedLevel()
    {
        return PlayerPrefs.GetInt(reachedLevelKey);
    }

    public static void SaveLevelProgress()
    {
        int currentLevel = SceneController.Instance.GetCurrentLevelIndex();
        if (currentLevel != -1 && currentLevel > GetReachedLevel()) PlayerPrefs.SetInt(reachedLevelKey, currentLevel);
    }
}
