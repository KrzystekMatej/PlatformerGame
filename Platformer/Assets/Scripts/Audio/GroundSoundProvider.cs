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

    public Sound GetSound(StateType actionType)
    {
        switch (actionType)
        {
            case StateType.Walk:
                return step;
            case StateType.Jump:
                return jump;
            case StateType.Fall:
                return land;
            default:
                return null;
        }
    }
}
