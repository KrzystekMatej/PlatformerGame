using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargeter : Targeter
{
    [SerializeField]
    private float arriveRadius;
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

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();

        Vector2 goalPosition = !isFleeing ? GoalPosition : agent.CenterPosition + (agent.CenterPosition - GoalPosition);

        if (Vector2.Distance(agent.CenterPosition, GoalPosition) > arriveRadius)
        {
            goal.Position = goalPosition;
            goal.Owner = GoalOwner;
        }

        GoalPosition = isPositionCached ? goalPosition : agent.CenterPosition;
        return goal;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GoalPosition, arriveRadius);
    }
}
