using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingActuator : Actuator
{
    [SerializeField]
    Path path;
    [SerializeField]
    private float predictTime = 0.1f;

    public override SteeringGoal GetFeasibleGoal(Agent agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        if (goal.HasPosition && pointPath.Count == 0)
        {
            pointPath.Add(agent.CenterPosition);
            pointPath.Add(goal.Position);
        }

        return goal;
    }

    public override Vector2 GetSteering(Agent agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        if (pointPath == null || pointPath.Count <= 1)
        {
            if (path.Points == null || path.Points.Count <= 1)
            {
                return new Vector2(0, 0);
            }
        }
        else
        {
            path.SetPoints(pointPath);
        }

        Vector2 futurePosition = agent.CenterPosition + agent.RigidBody.velocity * predictTime;

        path.CalculateTarget(futurePosition);
        Vector2 target = path.Target != null ? (Vector2)path.Target : futurePosition;

        Vector2 desiredVelocity = (agent.CenterPosition - target).normalized * agent.InstanceData.MaxForce;

        return (desiredVelocity - agent.RigidBody.velocity).normalized;
    }
}
