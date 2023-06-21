using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ChangeMaxSpeed : ActionNode
{
    [SerializeField]
    private float maxSpeed = 2f;

    protected override State OnUpdate()
    {
        context.agent.InstanceData.MaxSpeed = maxSpeed;
        return State.Success;
    }
}
