using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using DG.Tweening;

[System.Serializable]
public class TurnAround : ActionNode
{
    protected override State OnUpdate()
    {
        blackboard.DataTable["HorizontalDirection"] = -(float)blackboard.DataTable["HorizontalDirection"];
        return State.Success;
    }
}
