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

    public static void SaveReachedLevel(int reachedLevel)
    {
        PlayerPrefs.SetInt(reachedLevelKey, reachedLevel);
    }
}
