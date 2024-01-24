using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimpleActuator : Actuator
{
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
        if (!goal.HasPosition)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = (goal.Position - agent.CenterPosition).normalized * agent.InstanceData.MaxForce;

        return (desiredVelocity - agent.RigidBody.velocity).normalized;
    }
}
