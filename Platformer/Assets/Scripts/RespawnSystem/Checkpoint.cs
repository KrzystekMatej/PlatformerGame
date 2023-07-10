using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    private GameObject respawnTarget;
    private AudioSource audioSource;
    private SpawnSystem respawnSystem;
    private TriggerDetector triggerDetector;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        respawnSystem = GetComponentInParent<SpawnSystem>();
        triggerDetector = GetComponent<TriggerDetector>();
    }


    public void ActivateCheckpoint(Collider2D collision)
    {
        audioSource.Play();
        respawnTarget = collision.gameObject;
        triggerDetector.Disable();
        respawnTarget.GetComponent<Agent>().OnRespawnRequired.RemoveAllListeners();
        respawnTarget.GetComponent<Agent>().OnRespawnRequired.AddListener(RespawnPlayer);
        foreach (BackgroundController controller in respawnSystem.BackgroundControllers)
        {
            controller.CacheBackgroundData();
        }
    }

    private void RespawnPlayer()
    {
        respawnTarget.transform.position = transform.position;
        respawnTarget.SetActive(true);
        foreach (BackgroundController controller in respawnSystem.BackgroundControllers)
        {
            controller.RestoreBackgroundData();
        }
    }

    public void ResetCheckpoint()
    {
        respawnTarget = null;
        triggerDetector.Enable();
    }
}
