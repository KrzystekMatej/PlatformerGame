using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningPoint : MonoBehaviour
{
    [SerializeField]
    private float waitDuration = 1;
    private SceneController sceneController;
    private AudioSource audioSource;

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        audioSource = GetComponent<AudioSource>();
    }


    public void WinningPointReached()
    {
        audioSource.Play();
        StartCoroutine(LoadNextScene());
    }


    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(waitDuration);
        sceneController.LoadNextLevel();
    }
}
