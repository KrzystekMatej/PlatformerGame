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
    public Vector2 TargetPosition { get; set; }

    private void Start()
    {
        Agent agent = GetComponentInParent<AIManager>().Agent;
        TargetPosition = agent.CenterPosition;
    }

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();

        Vector2 goalPosition = !isFleeing ? TargetPosition : agent.CenterPosition + (agent.CenterPosition - TargetPosition);

        if (Vector2.Distance(agent.CenterPosition, TargetPosition) > arriveRadius)
        {
            goal.Position = goalPosition;
        }

        TargetPosition = isPositionCached ? goalPosition : agent.CenterPosition;
        return goal;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(TargetPosition, arriveRadius);
    }
}
