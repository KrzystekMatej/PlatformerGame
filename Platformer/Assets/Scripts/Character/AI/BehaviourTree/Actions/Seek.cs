using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;

public class Seek : ActionNode
{
    [SerializeField]
    private NodeProperty<float> maxSeekDistance;
    [SerializeField]
    private NodeProperty<SeekTargeter> seekTargeter;
    [SerializeField]
    private NodeProperty<Collider2D> goalCollider;
    [SerializeField]
    private NodeProperty<Vector2> goalPosition;

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        if (maxSeekDistance.Value >= 0) seekTargeter.Value.MaxSeekDistance = maxSeekDistance.Value;
        if (goalCollider.IsBlackboardKey())
        {
            seekTargeter.Value.GoalPosition = goalCollider.Value.bounds.center;
            seekTargeter.Value.GoalOwner = goalCollider.Value.gameObject;
        }
        else seekTargeter.Value.GoalOwner = null;

        if (goalPosition.IsBlackboardKey())
        {
            seekTargeter.Value.GoalPosition = goalPosition.Value;
        }

        return context.Steering.Recalculate();
    }

    protected override void OnStop() { }
}
