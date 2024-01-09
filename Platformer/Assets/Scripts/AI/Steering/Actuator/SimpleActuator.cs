using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimpleActuator : Actuator
{
    public override List<Vector2> GetPath(Agent agent, SteeringGoal goal)
    {
        if (goal.HasPosition)
        {
            return new List<Vector2>
            {
                goal.Position
            };
        }

        return null;
    }

    public override Vector2 GetSteering(Agent agent, List<Vector2> path, SteeringGoal goal)
    {
        if (path == null || path.Count == 0)
        {
            return new Vector2(0, 0);
        }

        Vector2 desiredVelocity = (path[0] - (Vector2)agent.GetCenterPosition()).normalized * agent.InstanceData.MaxForce;

        return (desiredVelocity - agent.RigidBody.velocity).normalized;
    }
}
