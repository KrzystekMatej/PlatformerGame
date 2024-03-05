using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    private Agent respawnTarget;
    private RespawnSystem respawnSystem;
    private TriggerDetector triggerDetector;


    private void Awake()
    {
        respawnSystem = GetComponentInParent<RespawnSystem>();
        triggerDetector = GetComponent<TriggerDetector>();
    }


    public void ActivateCheckpoint(Collider2D collision)
    {
        triggerDetector.Disable();
        respawnTarget = collision.GetComponent<Agent>();
        respawnTarget.OnRespawnRequired.RemoveAllListeners();
        respawnTarget.OnRespawnRequired.AddListener(RespawnPlayer);

        foreach (BackgroundController controller in respawnSystem.BackgroundControllers)
        {
            if (controller.IsFollowed(respawnTarget)) controller.CacheBackgroundData();
        }
    }

    private void RespawnPlayer()
    {
        respawnTarget.transform.position = transform.position;

        foreach (BackgroundController controller in respawnSystem.BackgroundControllers)
        {
            if (controller.IsFollowed(respawnTarget)) controller.RestoreBackgroundData();
        }
    }

    public void ResetCheckpoint()
    {
        respawnTarget = null;
        triggerDetector.Enable();
    }
}
