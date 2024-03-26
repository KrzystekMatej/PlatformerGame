using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel, selectLevelPanel;
    [SerializeField]
    private List<Button> levels = new List<Button>();

    private void Start()
    {
        InitSelectLevelMenu();
    }

    public void SelectLevelClicked()
    {
        mainPanel.SetActive(false);
        selectLevelPanel.SetActive(true);
    }

    public void MainMenuClicked()
    {
        mainPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
    }

    public void InitSelectLevelMenu()
    {
        int reachedLevel = SaveManager.Instance.GetReachedLevel();
        for (int i = levels.Count-1; i > reachedLevel; i--)
        {
            levels[i].interactable = false;
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
}
