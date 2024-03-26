using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : GlobalComponent<SaveManager>
{
    [SerializeField]
    private string reachedLevelKey;

    public int GetReachedLevel()
    {
        return PlayerPrefs.GetInt(reachedLevelKey);
    }

    public void SaveLevelProgress()
    {
        int currentLevel = SceneController.Instance.GetCurrentLevelIndex();
        if (currentLevel != -1 && currentLevel > GetReachedLevel()) PlayerPrefs.SetInt(reachedLevelKey, currentLevel);
    }
}
