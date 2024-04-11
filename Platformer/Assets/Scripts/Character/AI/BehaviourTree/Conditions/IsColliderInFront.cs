using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsColliderInFront : ConditionNode
{
    [SerializeField]
    NodeProperty<Collider2D> collider;

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        Vector2 attackerPosition = collider.Value.bounds.center;
        Vector2 orientation = context.Agent.OrientationController.CurrentOrientation;
        Vector2 agentPosition = context.Agent.CenterPosition;

        return Vector2.Dot(attackerPosition - agentPosition, orientation) >= 0;
    }

    protected override void OnStop() { }
}
