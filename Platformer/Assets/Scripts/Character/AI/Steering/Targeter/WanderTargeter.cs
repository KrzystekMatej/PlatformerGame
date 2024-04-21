using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class WanderTargeter : Targeter
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

    public override ProcessState Target(SteeringGoal goal)
    {
        wanderOrientation += MathUtility.GetRandomBinomial() * Mathf.PI * wanderRate;
        float agentOrientation = MathUtility.GetVectorRadAngle(agent.RigidBody.velocity);
        float targetOrientation = wanderOrientation + agentOrientation;
        goal.Position = agent.CenterPosition + MathUtility.PolarCoordinatesToVector2(agentOrientation, wanderOffset);

#if UNITY_EDITOR
        gizmoWanderCenterPosition = goal.Position;
#endif

        goal.Position += MathUtility.PolarCoordinatesToVector2(targetOrientation, wanderRadius);

#if UNITY_EDITOR
        gizmoGoalPosition = goal.Position;
#endif

        return ProcessState.Running;
    }


#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gizmoWanderCenterPosition, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.2f);
    }
#endif
}
