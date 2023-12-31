using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioFeedback : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<SoundKey, Sound> soundTable;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();     
    }

    public void PlaySound(SoundActionType actionType, int layerIndex)
    {
        SoundKey key = new SoundKey(actionType, Utility.IndexToLayerMask(layerIndex));
        if (soundTable.ContainsKey(key))
        {
            Sound sound = soundTable[key];
            audioSource.volume = sound.Volume;
            audioSource.PlayOneShot(sound.AudioClip);
        }
    }

    public void PlaySpecificSound(Sound sound)
    {
        if (sound != null)
        {
            audioSource.volume = sound.Volume;
            audioSource.PlayOneShot(sound.AudioClip);
        }
    }

    public void PlayRandomSound(List<Sound> sounds)
    {
        if (sounds?.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, sounds.Count);
            audioSource.volume = sounds[randomIndex].Volume;
            audioSource.PlayOneShot(sounds[randomIndex].AudioClip);
        }
    }
}