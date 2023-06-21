using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsOpponentBehind : Condition
{
    protected override bool IsConditionSatisfied()
    {
        GameObject opponent = (GameObject)blackboard.DataTable["opponent"];
        float orientation = context.agent.OrientationController.CurrentOrientation;
        return (orientation == 1 && opponent.transform.position.x < context.agent.transform.position.x)
            || (orientation == -1 && opponent.transform.position.x > context.agent.transform.position.x);
    }
}
