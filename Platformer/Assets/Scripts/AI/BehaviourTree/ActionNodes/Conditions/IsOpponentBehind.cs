using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsOpponentBehind : Condition
{
    protected override bool IsConditionSatisfied()
    {
        Vector3 opponentPosition = (Vector3)blackboard.DataTable["OpponentPosition"];
        float orientation = context.Agent.OrientationController.CurrentOrientation;
        return (orientation == 1 && opponentPosition.x < context.Agent.GetCenterPosition().x)
            || (orientation == -1 && opponentPosition.x > context.Agent.GetCenterPosition().x);
    }
}
