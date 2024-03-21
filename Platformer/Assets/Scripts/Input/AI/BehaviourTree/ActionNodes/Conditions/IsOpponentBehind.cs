using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsOpponentBehind : Condition
{
    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        Vector2 opponentPosition = blackboard.GetValue<Vector2>("OpponentPosition");
        float orientation = context.Agent.OrientationController.CurrentOrientation;
        return (orientation == 1 && opponentPosition.x < context.Agent.CenterPosition.x)
            || (orientation == -1 && opponentPosition.x > context.Agent.CenterPosition.x);
    }

    protected override void OnStop() { }
}
