using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetDefaultSpeed : ActionNode
{
    protected override State OnUpdate()
    {

        context.agent.InstanceData.MaxSpeed = context.agent.DefaultData.MaxSpeed;
        return State.Success;
    }
}
