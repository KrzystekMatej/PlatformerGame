using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ArriveTargeter : Targeter
{
    [SerializeField]
    private float arriveRadius = 0.5f;
    [SerializeField]
    private float slowRadius;

#if UNITY_EDITOR
    private float gizmoArriveRadius;
    private Vector2 gizmoGoalPosition;
#endif

    public override ProcessState Target(SteeringGoal goal)
    {
        float distance = Vector2.Distance(agent.PhysicsCenter, goal.Position);
        float arriveRadius = this.arriveRadius;

        if (goal.HasOwner)
        {
            Collider2D ownerCollider = goal.Owner.GetComponent<Collider2D>();
            if (ownerCollider)
            {
                float enclosingRadius = MathUtility.GetEnclosingCircleRadius(ownerCollider);
                if (enclosingRadius > arriveRadius) arriveRadius = enclosingRadius;
            }
        }

        if (distance <= arriveRadius) return ProcessState.Success;


#if UNITY_EDITOR
        gizmoArriveRadius = arriveRadius;
        gizmoGoalPosition = goal.Position;
#endif


        if (slowRadius != 0 && distance <= slowRadius) goal.Speed = agent.InstanceData.MaxSpeed * distance / slowRadius;
        else goal.Speed = agent.InstanceData.MaxSpeed;

        return ProcessState.Running;
    }
#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        if (gizmoArriveRadius > 0) Gizmos.DrawWireSphere(gizmoGoalPosition, gizmoArriveRadius);
        Gizmos.DrawWireSphere(gizmoGoalPosition, gizmoArriveRadius + slowRadius);
    }
#endif
}