using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SeekTargeter : Targeter
{
    [field: SerializeField]
    public float MaxSeekDistance = float.PositiveInfinity;
    [SerializeField]
    private bool isFleeing;

    public Vector2 GoalPosition { get; set; }
    public GameObject GoalOwner { get; set; }

    public override ProcessState TryUpdateGoal(SteeringGoal goal)
    {
        Vector2 goalPosition = isFleeing ? agent.CenterPosition + (agent.CenterPosition - GoalPosition) : GoalPosition;
        if (Vector2.Distance(goalPosition, agent.CenterPosition) > MaxSeekDistance) return ProcessState.Failure;
        goal.Position = goalPosition;
        goal.Owner = GoalOwner;
        return ProcessState.Running;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GoalPosition, 0.5f);
    }
}
