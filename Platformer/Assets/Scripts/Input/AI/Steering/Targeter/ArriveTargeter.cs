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
    [SerializeField]
    SeekTargeter seekTargeter;

#if UNITY_EDITOR
    private float gizmoArriveRadius;
#endif

    private void Awake()
    {
        if (seekTargeter == null) seekTargeter = GetComponent<SeekTargeter>();
    }

    public override bool TryUpdateGoal(AgentManager agent, SteeringGoal goal)
    {
        float distance = Vector2.Distance(agent.CenterPosition, seekTargeter.GoalPosition);
        float arriveRadius = agent.EnclosingCircleRadius;

        if (seekTargeter.GoalOwner != null)
        {
            Collider2D ownerCollider = seekTargeter.GoalOwner.GetComponent<Collider2D>();
            if (ownerCollider != null) arriveRadius += MathUtility.GetEnclosingCircleRadius(ownerCollider);
        }

#if UNITY_EDITOR
        gizmoArriveRadius = arriveRadius;
#endif

        if (distance <= arriveRadius)
        {
            return true;
        }

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
        if (gizmoArriveRadius > 0) Gizmos.DrawWireSphere(seekTargeter.GoalPosition, gizmoArriveRadius);
        Gizmos.DrawWireSphere(seekTargeter.GoalPosition, gizmoArriveRadius + slowRadius);
    }
}