using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargeter : Targeter
{
    [SerializeField]
    private bool isFleeing;
    [SerializeField]
    private bool isPositionCached;
    public Vector2 GoalPosition { get; set; }
    public GameObject GoalOwner { get; set; }


    private void Start()
    {
        GoalPosition = GetComponentInParent<AIManager>().Agent.CenterPosition;
    }

    public override bool TryUpdateGoal(Agent agent, SteeringGoal goal)
    {
        goal.Position = !isFleeing ? GoalPosition : agent.CenterPosition + (agent.CenterPosition - GoalPosition);
        goal.Owner = GoalOwner;

        GoalPosition = isPositionCached ? goal.Position : agent.CenterPosition;
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GoalPosition, 0.1f);
    }
}
