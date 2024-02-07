using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingTargeter : Targeter
{
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();
    [SerializeField]
    private Path path;
    [SerializeField]
    private bool isDynamic = false;

#if UNITY_EDITOR
    private Vector2 gizmoGoalPosition;
#endif

    public void Start()
    {
        RecalculatePath(GetComponentInParent<AIManager>().Agent);
    }

    public void RecalculatePath(Agent agent)
    {
        path.SetPoints(waypoints);
        path.Recalculate(agent);
    }

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();
        if (isDynamic) path.SetPoints(waypoints);

        goal.Position = path.CalculateGoal(agent);
#if UNITY_EDITOR
        gizmoGoalPosition = goal.Position;
#endif

        return goal;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        path.DrawGizmos();
    }
}