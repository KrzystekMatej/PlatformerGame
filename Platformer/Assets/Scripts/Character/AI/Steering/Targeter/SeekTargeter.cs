using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TheKiwiCoder;
using UnityEngine;

public class SeekTargeter : Targeter
{
    [field: SerializeField]
    public float MaxSeekDistance = float.PositiveInfinity;
    [SerializeField]
    private bool isFleeing;

    public Vector2 GoalPosition { get; set; }
    public GameObject GoalOwner { get; set; }

#if UNITY_EDITOR
    Vector2 gizmoGoalPosition;
#endif

    public override ProcessState Target(SteeringGoal goal)
    {
        if (!isFleeing)
        {
            goal.Position = GoalPosition;
            if (Vector2.Distance(goal.Position, agent.CenterPosition) > MaxSeekDistance) return ProcessState.Failure;
            goal.Owner = GoalOwner;
        }
        else
        {
            const float longDistance = 5000f;
            float fleeDistance = MaxSeekDistance > longDistance ? longDistance : MaxSeekDistance;
            goal.Position = GoalPosition + (agent.CenterPosition - GoalPosition).normalized * fleeDistance;
            goal.Owner = null;
        }
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
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.5f);
    }
#endif
}
