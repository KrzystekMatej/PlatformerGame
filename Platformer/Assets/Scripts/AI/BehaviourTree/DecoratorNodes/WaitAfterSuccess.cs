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

    protected override void OnStart()
    {
        state = child.Update();
        if (state == NodeState.Success)
        {
            startTime = Time.time;
            state = NodeState.Failure;
        }
    }

    protected override NodeState OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return NodeState.Success;
        }
        return state;
    }

    protected override void OnStop() { }
}
