using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class WalkForward : ActionNode
{
    public override void OnInit()
    {
        float currentHorizontalInput = context.Agent.InputController.InputData.SteeringForce.x;
        float currentAgentOrientation = context.Agent.OrientationController.CurrentOrientation;
        blackboard.SetValue("HorizontalDirection", currentHorizontalInput != 0 ? currentHorizontalInput : currentAgentOrientation);
    }

    protected override void OnStart() { }


    protected override NodeState OnUpdate()
    {
        context.InputController.AddSteeringForce(new Vector2(blackboard.GetValue<float>("HorizontalDirection") * context.Agent.InstanceData.MaxForce, 0));
        return NodeState.Success;
    }

    protected override void OnStop() { }
}
