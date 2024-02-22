using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SimpleActuator : Actuator
{
#if UNITY_EDITOR
    private Vector2 gizmoDesiredVelocity;
    private Vector2 gizmoAgentCenterPosition;
#endif

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

    public override Vector2? GetSteering(Agent agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        if (!goal.HasPosition)
        {
            return null;
        }

        Vector2 desiredVelocity;
        Vector2 goalDirection = (goal.Position - agent.CenterPosition).normalized;

        if (goal.HasSpeed) desiredVelocity = goalDirection * goal.Speed;
        else desiredVelocity = goalDirection * agent.InstanceData.MaxSpeed;

#if UNITY_EDITOR
        gizmoDesiredVelocity = desiredVelocity;
        gizmoAgentCenterPosition = agent.CenterPosition;
#endif

        return desiredVelocity - agent.RigidBody.velocity;
    }


    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(gizmoAgentCenterPosition, gizmoAgentCenterPosition + gizmoDesiredVelocity);
    }
}
