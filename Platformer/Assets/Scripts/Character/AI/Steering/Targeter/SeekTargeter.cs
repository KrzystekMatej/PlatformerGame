using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargeter : Targeter
{
    [SerializeField]
    private bool isFleeing;

    public Vector2 GoalPosition { get; set; }
    public GameObject GoalOwner { get; set; }

    public override bool TryUpdateGoal(SteeringGoal goal)
    {
        goal.Position = isFleeing ? agent.CenterPosition + (agent.CenterPosition - GoalPosition) : GoalPosition;
        goal.Owner = GoalOwner;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GoalPosition, 0.1f);
    }
}
