using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using DG.Tweening;

[System.Serializable]
public class TurnAround : ActionNode
{
    [SerializeField]
    private NodeProperty<SeekTargeter> seekTargeter;
    private Vector2 turnedDirection;

    protected override void OnStart()
    {
        const float lookAhead = 2f;
        turnedDirection = - context.Agent.OrientationController.CurrentOrientation;
        seekTargeter.Value.GoalPosition = context.Agent.CenterPosition + turnedDirection * context.Agent.EnclosingCircleRadius * lookAhead;
    }

    protected override ProcessState OnUpdate()
    {
        const float safetyTurnValue = 0.99f;
        if (Vector2.Dot(context.Agent.OrientationController.CurrentOrientation, turnedDirection) >= safetyTurnValue)
        {
            return ProcessState.Success;
        }
        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
