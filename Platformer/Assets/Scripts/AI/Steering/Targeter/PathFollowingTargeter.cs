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

    public void Awake()
    {
        path.SetPoints(waypoints);
    }

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();
        if (isDynamic) path.SetPoints(waypoints);

        Vector2 futurePosition = (Vector2)agent.GetCenterPosition() + agent.RigidBody.velocity * predictTime;

        path.CalculateTarget(futurePosition);
        goal.Position = path.Target;

        return goal;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Agent agent = GetComponentInParent<AIManager>().Agent;
        path.DrawGizmos(agent.GetCenterPosition());
    }
}