using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehaviour : MonoBehaviour, ISteeringBehaviour
{
    [field: SerializeField]
    public bool IsFleeing { get; set; } = false;
    [SerializeField]
    private string targetDetectorName;

    public virtual Vector2 GetSteering(Agent agent, Vision vision)
    {
        Collider2D target = GetTarget(vision);
        return CalculateSteeringForce(agent, target.bounds.center);
    }


    public Collider2D GetTarget(Vision vision)
    {
        AreaDetector areaDetector = (AreaDetector)vision.GetDetector(targetDetectorName);
        return areaDetector.GetColliders()[0];
    }


    public Vector2 CalculateSteeringForce(Agent agent, Vector2 targetPosition)
    {
        Vector2 desiredVelocity = (targetPosition - (Vector2)transform.position).normalized * agent.InstanceData.MaxForce;
        Vector2 steeringForce = (desiredVelocity - agent.RigidBody.velocity).normalized;

        return IsFleeing ? -steeringForce : steeringForce;
    }
}
