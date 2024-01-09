using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingTargeter : Targeter
{
    [SerializeField]
    private Path path;
    [SerializeField]
    private float pathOffset;
    [SerializeField]
    private float predictTime = 0.1f;
    private int lastClosestPointIndex;
    private float currentParameter;

    private void Awake()
    {
        path.Initialize();
    }

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();

        Vector2 futurePosition = (Vector2)agent.GetCenterPosition() + agent.RigidBody.velocity * predictTime;

        (currentParameter, lastClosestPointIndex) = path.GetCloseParameter(futurePosition, lastClosestPointIndex);
        float targetParameter = currentParameter + pathOffset;
        Debug.Log(targetParameter);

        goal.Position = path.GetPosition(targetParameter, lastClosestPointIndex);

        return goal;
    }
}
