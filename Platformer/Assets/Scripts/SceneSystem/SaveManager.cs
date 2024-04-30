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

    public void SaveLevelProgress(int toSave)
    {
        if (toSave > GetReachedLevel()) PlayerPrefs.SetInt(reachedLevelKey, toSave);
    }
}
