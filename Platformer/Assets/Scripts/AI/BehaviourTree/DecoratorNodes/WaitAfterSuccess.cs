using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class WaitAfterSuccess : DecoratorNode
{
    [SerializeField]
    private float duration = 1.0f;
    private float startTime;

    public override void Initialize()
    {
        startTime = -duration;
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            State state = child.Update();
            if (state == State.Success)
            {
                startTime = Time.time;
            }
            return state;
        }
        return State.Failure;
    }
}
