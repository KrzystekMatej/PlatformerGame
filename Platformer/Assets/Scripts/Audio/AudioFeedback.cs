using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioFeedback : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();     
    }

    public void PlaySpecificSound(Sound sound)
    {
        if (sound != null)
        {
            audioSource.volume = sound.Volume;
            audioSource.PlayOneShot(sound.AudioClip);
        }
    }
}