using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveTargeter : Targeter
{
    [SerializeField]
    private float slowRadius;
    [SerializeField]
    private float slowDegree = 1;
    [SerializeField]
    private float minSpeed = 0.1f;

    public Vector2 ArrivePosition { get; private set; }

#if UNITY_EDITOR
    private float gizmoArriveRadius;
    private Vector2 gizmoGoalPosition;
#endif

    public override bool TryUpdateGoal(AgentManager agent, SteeringGoal goal)
    {
        float distance = Vector2.Distance(agent.CenterPosition, goal.Position);
        float arriveRadius = agent.EnclosingCircleRadius;

        if (goal.HasOwner)
        {
            Collider2D ownerCollider = goal.Owner.GetComponent<Collider2D>();
            //if (ownerCollider != null) arriveRadius += MathUtility.GetEnclosingCircleRadius(ownerCollider);
        }

        if (distance <= arriveRadius) return true;
        
        ArrivePosition = goal.Position + (agent.CenterPosition - goal.Position) * (arriveRadius / distance);

#if UNITY_EDITOR
        gizmoArriveRadius = arriveRadius;
        gizmoGoalPosition = goal.Position;
#endif


        if (slowRadius != 0 && distance <= arriveRadius + slowRadius) goal.Speed = agent.InstanceData.MaxSpeed * Mathf.Pow((distance - arriveRadius) / slowRadius, slowDegree);
        else goal.Speed = agent.InstanceData.MaxSpeed;

        if (goal.Speed <= minSpeed)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        if (gizmoArriveRadius > 0) Gizmos.DrawWireSphere(gizmoGoalPosition, gizmoArriveRadius);
        Gizmos.DrawWireSphere(gizmoGoalPosition, gizmoArriveRadius + slowRadius);
    }
}