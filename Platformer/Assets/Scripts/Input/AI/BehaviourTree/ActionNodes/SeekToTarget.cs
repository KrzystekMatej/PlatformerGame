using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;

public class SeekToTarget : ActionNode
{
    [SerializeField]
    private float maxSeekDistance = 20;

    private SeekTargeter seekTargeter;
    private Collider2D goal;
    private bool stoppedSeeking;

    protected override void OnStart()
    {
        seekTargeter = blackboard.GetValue<SeekTargeter>(context.Steering.CurrentPipelineName + nameof(SeekTargeter));
        List<RaycastHit2D> checkedHits = blackboard.GetValue<List<RaycastHit2D>>("CheckedHits");
        float minDistance = float.PositiveInfinity;
        foreach (var hit in checkedHits)
        {
            float distance = Vector2.Distance(hit.collider.bounds.center, context.Agent.CenterPosition);
            if (distance < minDistance) goal = hit.collider;
        }

    }

    protected override ProcessState OnUpdate()
    {
        if (Vector2.Distance(context.Agent.CenterPosition, goal.bounds.center) <= maxSeekDistance)
        {
            seekTargeter.GoalPosition = goal.bounds.center;
            seekTargeter.GoalOwner = goal.gameObject;
            return ProcessState.Running;
        }

        return context.Steering.State;
    }

    protected override void OnStop() { }
}
