using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderTargeter : SeekTargeter
{
    [SerializeField]
    private float wanderOffset;
    [SerializeField]
    private float wanderRadius;
    [SerializeField]
    private float wanderRate;

    private float wanderOrientation;
#if UNITY_EDITOR
    private Vector2 gizmoWanderCenterPosition;
    private Vector2 gizmoGoalPosition;
#endif

    public override bool TryUpdateGoal(Agent agent, SteeringGoal goal)
    {
        wanderOrientation += MathUtility.GetRandomBinomial() * Mathf.PI * wanderRate;
        float agentOrientation = MathUtility.GetVectorAngle(agent.RigidBody.velocity);
        float targetOrientation = wanderOrientation + agentOrientation;
        GoalPosition = agent.CenterPosition + MathUtility.PolarCoordinatesToVector2(agentOrientation, wanderOffset);

#if UNITY_EDITOR
        gizmoWanderCenterPosition = GoalPosition;
#endif

        GoalPosition += MathUtility.PolarCoordinatesToVector2(targetOrientation, wanderRadius);

#if UNITY_EDITOR
        gizmoGoalPosition = GoalPosition;
#endif

        return base.TryUpdateGoal(agent, goal);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gizmoWanderCenterPosition, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.2f);
    }
}
