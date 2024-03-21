using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private bool playActivationSound = true;
    private AgentManager respawnTarget;
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
        respawnTarget = collision.GetComponent<AgentManager>();
        respawnTarget.OnRespawnRequired.RemoveAllListeners();
        respawnTarget.OnRespawnRequired.AddListener(RespawnPlayer);

        foreach (BackgroundController controller in respawnSystem.BackgroundControllers)
        {
            if (controller.IsFollowed(respawnTarget)) controller.CacheBackgroundData();
        }

        if (playActivationSound) respawnTarget.AudioFeedback.PlaySpecificSound(respawnSystem.ActivationSound);
    }

    private void RespawnPlayer()
    {
        respawnTarget.transform.position = GetComponent<Collider2D>().bounds.center;

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
