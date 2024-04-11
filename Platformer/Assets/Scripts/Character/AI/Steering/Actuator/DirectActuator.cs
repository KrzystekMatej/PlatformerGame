using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DirectActuator : Actuator
{
    [SerializeField]
    private float timeToTarget = 0.1f;
#if UNITY_EDITOR
    private Vector2 gizmoDesiredVelocity;
    private Vector2 gizmoAgentCenterPosition;
#endif

    public override List<Vector2> GetPath(SteeringGoal goal)
    {
        List<Vector2> path = new List<Vector2>();

        if (goal.HasPosition)
        {
            path.Add(agent.CenterPosition);
            path.Add(goal.Position);
        }

        return path;
    }

    public override Vector2? GetSteering(List<Vector2> pointPath, SteeringGoal goal)
    {
        if (pointPath.Count < 2) return null;

        Vector2 goalDirection = (pointPath[1] - pointPath[0]).normalized;

        if (goal.HasSpeed)
        {
            Vector2 targetVelocity = goalDirection * goal.Speed;
            Vector2 force = targetVelocity - agent.RigidBody.velocity;
            return force / timeToTarget;
        }
        else return goalDirection * agent.InstanceData.MaxForce;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(gizmoAgentCenterPosition, gizmoAgentCenterPosition + gizmoDesiredVelocity);
    }
#endif
}
