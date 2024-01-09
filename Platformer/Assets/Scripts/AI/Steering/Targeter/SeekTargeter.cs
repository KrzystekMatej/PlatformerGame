using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargeter : Targeter
{
    [SerializeField]
    protected bool isFleeing;
    [SerializeField]
    private OverlapDetector targetDetector;

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();
        Collider2D target = GetTarget(agent);

        if (target != null)
        {
            goal.Position = !isFleeing ? target.bounds.center : agent.GetCenterPosition() + (agent.GetCenterPosition() - target.bounds.center);
        }

        return goal;
    }

    public Collider2D GetTarget(Agent agent)
    {
        int detectionCount = targetDetector.Detect(agent.GetCenterPosition());
        return detectionCount == 0 ? null : targetDetector.Colliders[0];
    }
}
