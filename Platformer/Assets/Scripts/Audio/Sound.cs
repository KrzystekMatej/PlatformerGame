using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "AudioFeedback/Sound")]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip AudioClip;
    [Range(0f, 1f)]
    public float Volume;
}

public enum SoundActionType
{
    Step,
    Jump,
    Land,
    Attack
}