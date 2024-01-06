using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehaviour : MonoBehaviour, ISteeringBehaviour
{
    [field: SerializeField]
    public bool IsFleeing { get; set; } = false;
    [SerializeField]
    private string targetDetectorName;
    [SerializeField]
    protected Collider2D target;

    public virtual Vector2 GetSteering(Agent agent, Vision vision)
    {
        return CalculateSteeringForce(agent, target.bounds.center);
    }

    public Vector2 CalculateSteeringForce(Agent agent, Vector2 targetPosition)
    {
        Vector2 desiredVelocity = (targetPosition - (Vector2)agent.GetCenterPosition()).normalized * agent.InstanceData.MaxForce;
        Vector2 steeringForce = (desiredVelocity - agent.RigidBody.velocity).normalized;

        return IsFleeing ? -steeringForce : steeringForce;
    }
}
