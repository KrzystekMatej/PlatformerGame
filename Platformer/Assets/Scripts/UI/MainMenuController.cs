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
        //foreach (Button button in levels)
        //{
        //    button.interactable = true;
        //}

        int reachedLevel = SaveManager.Instance.GetReachedLevel();
        for (int i = 0; i <= reachedLevel && i < levels.Count; i++)
        {
            levels[i].interactable = true;
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
