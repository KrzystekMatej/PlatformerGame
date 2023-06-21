using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    private GameObject respawnTarget;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            respawnTarget = collision.gameObject;
            GetComponent<Collider2D>().enabled = false;
            respawnTarget.GetComponent<Agent>().OnRespawnRequired.RemoveAllListeners();
            respawnTarget.GetComponent<Agent>().OnRespawnRequired.AddListener(RespawnPlayer);
        }
    }

    public void RespawnPlayer()
    {
        respawnTarget.transform.position = transform.position;
        respawnTarget.SetActive(true);
    }

    public void ResetCheckpoint()
    {
        respawnTarget = null;
        GetComponent<Collider2D>().enabled = true;
    }
}
