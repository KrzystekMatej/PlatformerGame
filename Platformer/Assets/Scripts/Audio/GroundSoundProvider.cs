using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSoundProvider : MonoBehaviour
{
    [SerializeField]
    private Sound step;
    [SerializeField]
    private Sound jump;
    [SerializeField]
    private Sound land;

    public Sound GetSound(SoundActionType actionType)
    {
        switch (actionType)
        {
            case SoundActionType.Step:
                return step;
            case SoundActionType.Jump:
                return jump;
            case SoundActionType.Land:
                return land;
            default:
                return null;
        }
    }
}
