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

    protected override NodeState OnUpdate()
    {
        NodeState state = child.Update();
        if (Time.time - startTime > duration)
        {

            return NodeState.Success;
        }

        if ((!repeatOnSuccess && state == NodeState.Success) || (!repeatOnFailure && state == NodeState.Failure))
        {
            return state;
        }

        return NodeState.Running;
    }

    protected override void OnStop()
    {

    }
}
