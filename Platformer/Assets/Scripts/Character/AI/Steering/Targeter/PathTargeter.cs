using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class PathTargeter : Targeter
{
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();
    [SerializeField]
    private Path path;
    [SerializeField]
    private bool isDynamic = false;


    public override void Enable()
    {
        Recalculate();
    }

    public void Recalculate()
    {
        path.SetPoints(waypoints);
        path.Recalculate(Agent);
    }

    public Vector2 GetPathTarget()
    {
        Recalculate();
        return path.CalculateGoalWithCoherence(Agent);
    }


    public override ProcessState Target(SteeringGoal goal)
    {
        if (isDynamic) Recalculate();

        goal.Position = path.CalculateGoalWithCoherence(Agent);
        
        return path.ReachedEnd(Agent, goal.Position) ? ProcessState.Success : ProcessState.Running;
    }

#if UNITY_EDITOR
    public override void DrawGizmos()
    {
        if (!Application.isPlaying) return;
        path.DrawAllGizmos();
    }
#endif
}