using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class RepeatUntil : DecoratorNode
{
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private bool repeatOnSuccess = true;
    [SerializeField]
    private bool repeatOnFailure = false;
    private float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override ProcessState OnUpdate()
    {
        ProcessState state = child.Update();
        if (Time.time - startTime > duration)
        {

            return ProcessState.Success;
        }

        if ((!repeatOnSuccess && state == ProcessState.Success) || (!repeatOnFailure && state == ProcessState.Failure))
        {
            return state;
        }

        return ProcessState.Running;
    }

    protected override void OnStop()
    {

    }
}
