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

    protected override State OnUpdate()
    {
        State state = child.Update();
        if (Time.time - startTime > duration)
        {

            return State.Success;
        }

        if (!repeatOnSuccess && state == State.Success)
        {
            return State.Failure;
        }
        else if (!repeatOnSuccess && state == State.Success)
        {
            return State.Failure;
        }

        return State.Running;
    }
}
