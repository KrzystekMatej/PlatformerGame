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
    [SerializeField]
    private Collider2D target;

    public Vector2 GetSteering(Agent agent, Vision vision)
    {
        Vector2 direction = target.bounds.center - agent.GetCenterPosition();
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

}
