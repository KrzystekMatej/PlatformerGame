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

    public static void SaveLevelProgress(SceneController sceneController)
    {
        int reachedLevel = sceneController.GetCurrentLevelIndex();
        if (reachedLevel != -1) PlayerPrefs.SetInt(reachedLevelKey, reachedLevel);
    }
}
