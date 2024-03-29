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

    public void Start()
    {
        RecalculatePath(GetComponentInParent<AIManager>().Agent);
    }

    public void RecalculatePath(AgentManager agent)
    {
        path.SetPoints(waypoints);
        path.Recalculate(agent);
    }

    public override bool TryUpdateGoal(AgentManager agent, SteeringGoal goal)
    {
        if (isDynamic) path.SetPoints(waypoints);

        goal.Position = path.CalculateGoalWithCoherence(agent);
        
        return path.ReachedEnd(agent, goal.Position);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        path.DrawAllGizmos();
    }
}