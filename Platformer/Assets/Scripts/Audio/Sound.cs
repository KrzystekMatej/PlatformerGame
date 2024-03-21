using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "AudioFeedback/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip AudioClip;
    public AudioMixerGroup Mixer;
    [Range(0f, 1f)]
    public float Volume;
}
