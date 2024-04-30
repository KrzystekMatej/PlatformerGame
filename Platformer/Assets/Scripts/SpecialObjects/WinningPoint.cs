using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningPoint : MonoBehaviour
{
    [SerializeField]
    private float waitDuration = 1;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void WinningPointReached()
    {
        audioSource.Play();
        SaveManager.Instance.SaveLevelProgress(SceneController.Instance.GetCurrentLevelIndex() + 1);
        StartCoroutine(LoadNextScene());
    }


    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(waitDuration);
        SceneController.Instance.LoadNextScene();
    }
}
