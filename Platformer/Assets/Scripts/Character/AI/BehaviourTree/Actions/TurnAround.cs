using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using DG.Tweening;

[System.Serializable]
public class TurnAround : ActionNode
{
    private SeekTargeter seekTargeter;
    private Vector2 turnedDirection;

    protected override void OnStart()
    {
        seekTargeter = blackboard.GetValue<SeekTargeter>(context.Steering.CurrentPipelineName + nameof(SeekTargeter));
        const float lookAhead = 2f;
        turnedDirection = - context.Agent.OrientationController.CurrentOrientation;
        seekTargeter.GoalPosition = context.Agent.CenterPosition + turnedDirection * context.Agent.EnclosingCircleRadius * lookAhead;
    }

    protected override ProcessState OnUpdate()
    {
        const float safetyTurnValue = 0.99f;
        if (Vector2.Dot(context.Agent.OrientationController.CurrentOrientation, turnedDirection) >= safetyTurnValue)
        {
            return ProcessState.Success;
        }
        return ProcessState.Running;
    }

    protected override void OnStop() { }
}
