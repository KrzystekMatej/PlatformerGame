using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Steering : MonoBehaviour
{
    [SerializeField]
    private List<ContextSteeringBehaviour> contextBehaviours = new List<ContextSteeringBehaviour>();
    [SerializeField]
    private int contextMapResolution = 8;
    [SerializeField]
    private Color gizmoColor = Color.yellow;
    [SerializeField]
    private float gizmoRayLength = 2;
    private Vector2 gizmoDirection = Vector2.zero;
    [SerializeField]
    private float dangerModifier = 1;
    [SerializeField]
    private float interestModifier = 1;

    private Agent agent;

    private List<Vector2> directions = new List<Vector2>();
    private float[] interest;
    private float[] danger;

    private void Awake()
    {
        agent = GetComponentInParent<Agent>();
        contextBehaviours = GetComponentsInChildren<ContextSteeringBehaviour>().ToList();
        InitializeContextMaps();
    }


    private void InitializeContextMaps()
    {
        danger = new float[contextMapResolution];
        interest = new float[contextMapResolution];

        float angleStep = 360 / contextMapResolution;
        float currentAngle = 0;

        for (int i = 0; i < contextMapResolution; i++)
        {
            directions.Add((Quaternion.Euler(0, 0, currentAngle) * Vector3.right).normalized);
            currentAngle += angleStep;
        }
    }

    private void ResetContextMaps()
    {
        for (int i = 0; i < contextMapResolution; i++)
        {
            danger[i] = 0f;
            interest[i] = 0f;
        }
    }

    public Vector2 Seek(Vector3 target)
    {
        Vector2 desiredVelocity = (target - agent.transform.position).normalized;
        gizmoDirection = desiredVelocity;
        return desiredVelocity - agent.InputController.InputData.MovementVector;
    }


    public Vector2 Flee(Vector3 target)
    {
        Vector2 desiredVelocity = (agent.transform.position - target).normalized * agent.InstanceData.MaxSpeed;
        gizmoDirection = desiredVelocity;
        return desiredVelocity - agent.RigidBody.velocity;
    }

    public Vector2 GetContextSteeringForce()
    {
        ResetContextMaps();
        foreach (ContextSteeringBehaviour behaviour in contextBehaviours)
        {
            behaviour.ModifySteeringContext(danger, interest, directions);
        }

        Vector2 desiredVelocity = Vector2.zero;
        for (int i = 0; i < contextMapResolution; i++)
        {
            desiredVelocity += Mathf.Clamp01(interest[i] * interestModifier - danger[i] * dangerModifier) * directions[i];
        }

        gizmoDirection = desiredVelocity;
        return desiredVelocity.normalized - agent.InputController.InputData.MovementVector;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawRay(transform.position, gizmoDirection * gizmoRayLength);
        }
    }
}
