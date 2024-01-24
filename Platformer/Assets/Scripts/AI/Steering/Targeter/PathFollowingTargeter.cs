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
    private float predictTime = 0.1f;
    [SerializeField]
    private bool isDynamic = false;

#if UNITY_EDITOR
    private Vector2 currentFuturePosition;
#endif

    public void Start()
    {
        RecalculatePath(GetComponentInParent<AIManager>().Agent);
    }

    public void RecalculatePath(Agent agent)
    {
        path.SetPoints(waypoints);

        Vector2 futurePosition = agent.CenterPosition + agent.RigidBody.velocity * predictTime;
        path.Recalculate(futurePosition);
    }

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();
        if (isDynamic) path.SetPoints(waypoints);

        Vector2 futurePosition = agent.CenterPosition + agent.RigidBody.velocity * predictTime;

        path.CalculateTarget(futurePosition);
        goal.Position = path.Target != null ? (Vector2)path.Target : futurePosition;

#if UNITY_EDITOR
        currentFuturePosition = futurePosition;
#endif

        return goal;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        path.DrawGizmos(currentFuturePosition);
    }
}