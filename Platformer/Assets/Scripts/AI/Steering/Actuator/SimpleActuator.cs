using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimpleActuator : Actuator
{
    public override List<Vector2> GetPath(Agent agent, SteeringGoal goal)
    {
        List<Vector2> path = new List<Vector2>();

        if (goal.HasPosition)
        {
            path.Add(agent.CenterPosition);
            path.Add(goal.Position);
        }

        return path;
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
