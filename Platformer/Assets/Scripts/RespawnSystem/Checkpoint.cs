using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Sound activationSound;
    private AgentManager respawnTarget;
    private TriggerFilter triggerDetector;
    private AudioSource audioSource;


    private void Awake()
    {
        triggerDetector = GetComponent<TriggerFilter>();
        audioSource = GetComponent<AudioSource>();
    }


    public void ActivateCheckpoint(Collider2D collision)
    {
        triggerDetector.Disable();
        respawnTarget = collision.GetComponent<AgentManager>();
        respawnTarget.OnRespawnRequired.RemoveAllListeners();
        respawnTarget.OnRespawnRequired.AddListener(RespawnPlayer);

        respawnTarget.AudioFeedback.PlaySpecificSound(activationSound);
    }

    private void RespawnPlayer()
    {
        respawnTarget.transform.position = GetComponent<Collider2D>().bounds.center;
    }

    public void ResetCheckpoint()
    {
        respawnTarget = null;
        triggerDetector.Enable();
    }
}
