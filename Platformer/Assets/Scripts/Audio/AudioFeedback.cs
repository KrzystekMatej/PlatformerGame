using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
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
            AudioMixerGroup defaultGroup = audioSource.outputAudioMixerGroup;
            audioSource.outputAudioMixerGroup = sound.Mixer ? sound.Mixer : defaultGroup;
            audioSource.volume = sound.Volume;
            audioSource.PlayOneShot(sound.AudioClip);
            audioSource.outputAudioMixerGroup = defaultGroup;
        }
    }
}