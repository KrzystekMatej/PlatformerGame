using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsAttackerBehind : ConditionNode
{
    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        Vector2 attackerPosition = blackboard.GetValue<Vector2>("AttackerPosition");
        Vector2 orientation = context.Agent.OrientationController.CurrentOrientation;
        Vector2 agentPosition = context.Agent.CenterPosition;

        return Vector2.Dot(attackerPosition - agentPosition, orientation) < 0;
    }

    protected override void OnStop() { }
}
