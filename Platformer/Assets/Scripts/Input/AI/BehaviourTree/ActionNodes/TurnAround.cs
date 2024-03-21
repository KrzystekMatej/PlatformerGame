using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using DG.Tweening;

[System.Serializable]
public class TurnAround : ActionNode
{
    private float turnedDirection;

    protected override void OnStart()
    {
        turnedDirection = -blackboard.GetValue<float>("HorizontalDirection");
    }

    protected override ProcessState OnUpdate()
    {
        blackboard.SetValue("HorizontalDirection", turnedDirection);
        context.InputController.AddSteeringForce(new Vector2(turnedDirection * context.Agent.InstanceData.MaxForce, 0f));
        if (context.Agent.OrientationController.CurrentOrientation == turnedDirection)
        {
            return ProcessState.Success;
        }
        return ProcessState.Running;
    }

    protected override void OnStop()
    {
        context.InputController.AddSteeringForce(Vector2.zero);
    }
}
