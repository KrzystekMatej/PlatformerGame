using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class WalkForward : ActionNode
{
    public override void OnInit()
    {
        blackboard.SetValue("HorizontalDirection", 1f);
    }

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        float direction = blackboard.GetValue<float>("HorizontalDirection");
        context.InputController.AddSteeringForce(new Vector2(direction * context.Agent.InstanceData.MaxForce, 0f));
        return ProcessState.Running;
    }

    protected override void OnStop()
    {
        context.InputController.AddSteeringForce(Vector2.zero);
    }
}
