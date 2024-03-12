using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using DG.Tweening;

[System.Serializable]
public class TurnAround : ActionNode
{
    protected override void OnStart() { }

    protected override NodeState OnUpdate()
    {
        blackboard.SetValue("HorizontalDirection", -blackboard.GetValue<float>("HorizontalDirection"));
        return NodeState.Success;
    }

    protected override void OnStop() { }
}
