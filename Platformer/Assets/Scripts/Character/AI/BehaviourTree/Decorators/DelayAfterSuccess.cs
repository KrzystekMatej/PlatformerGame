using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DelayAfterSuccess : DecoratorNode
{
    [SerializeField]
    private float duration = 1.0f;

    private float startTime = -1f;

    protected override void OnStart() {}

    protected override ProcessState OnUpdate()
    {
        if (startTime == -1)
        {
            state = child.Update();
        }
        else if (Time.time - startTime > duration)
        {
            startTime = -1f;
            return ProcessState.Success;
        }
        
        if (state == ProcessState.Success)
        {
            startTime = Time.time;
            state = ProcessState.Failure;
        }

        return state;
    }

    protected override void OnStop() { }
}
