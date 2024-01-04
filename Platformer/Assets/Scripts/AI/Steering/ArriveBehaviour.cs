using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveBehaviour : MonoBehaviour, ISteeringBehaviour
{
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private float slowRadius;
    [SerializeField]
    private float timeToTarget = 0.1f;
    [SerializeField]
    private string targetDetectorName;

    public Vector2 GetSteering(Agent agent, Vision vision)
    {
        Collider2D target = GetTarget(vision);

        Vector2 direction = target.bounds.center - transform.position;
        float distance = direction.magnitude;

        if (distance < targetRadius) return Vector2.zero;

        float targetSpeed = distance > slowRadius ? agent.InstanceData.MaxSpeed : agent.InstanceData.MaxSpeed * distance / slowRadius;

        Vector2 targetVelocity = direction.normalized * targetSpeed;

        Vector2 steeringForce = (targetVelocity - agent.RigidBody.velocity) / timeToTarget;

        if (steeringForce.magnitude > agent.InstanceData.MaxForce)
        {
            steeringForce = steeringForce.normalized * agent.InstanceData.MaxForce;
        }

        return steeringForce;
    }

    public Collider2D GetTarget(Vision vision)
    {
        AreaDetector areaDetector = (AreaDetector)vision.GetDetector(targetDetectorName);
        return areaDetector.GetColliders()[0];
    }

}
