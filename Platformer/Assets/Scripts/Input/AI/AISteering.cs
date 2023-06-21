using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteering : MonoBehaviour
{
    private Agent agent;
    private List<VisionRay> rays;
    private Vector2 steeringForce;
    public Vector2 SteeringForce { get => steeringForce; }

    private void Awake()
    {
        agent = GetComponentInParent<Agent>();
    }

    public void CalculateSteeringForce(Vector3 target)
    {
        steeringForce = Seek(target);
    }

    public Vector2 Seek(Vector3 target)
    {
        Vector2 desiredVelocity = (target - agent.transform.position).normalized;
        return desiredVelocity - agent.InputController.InputData.MovementVector;
    }


    private Vector2 Flee(Vector3 target)
    {
        Vector2 desiredVelocity = (agent.transform.position - target).normalized * agent.InstanceData.MaxSpeed;
        return desiredVelocity - agent.RigidBody.velocity;
    }

    private Vector2 CollisionAvoidance(RaycastHit2D[] hits)
    {
        return Vector2.zero;
    }

    public void ProvideRays(List<VisionRay> rays)
    {
        this.rays = rays;
    }
}
