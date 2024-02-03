using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WallConstraint : Constraint
{
    [SerializeField]
    private int maxRayCount;
    [SerializeField]
    private float maxRayLength;
    [SerializeField]
    private LayerMask wallLayerMask;
    [SerializeField]
    private float margin;


    private int rayCount;
    private float rayLength;
    private Vector2 pathDirection = Vector2.right;
#if UNITY_EDITOR
    private Vector2? gizmoChosenRay;
#endif

    private void Start()
    {
        Agent agent = GetComponentInParent<AIManager>().Agent;
        AdjustRayParameters(agent.CenterPosition, agent.EnclosingCircleRadius);
    }

    public override bool IsViolated(Agent agent, List<Vector2> pointPath)
    {
        pathDirection = (pointPath[1] - pointPath[0]).normalized;
        bool result = Physics2D.Raycast(pointPath[0], pathDirection, rayLength, wallLayerMask).collider != null;

        return result;
    }

    public override SteeringGoal Suggest(Agent agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        AdjustRayParameters(pointPath[0], agent.EnclosingCircleRadius);
        float angleStep = 360 / rayCount;

        Vector2? chosenRay = null;

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = (i % 2 == 0 ? 1 : -1) * (i / 2 * angleStep);
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * pathDirection;
            RaycastHit2D hit = Physics2D.Raycast(pointPath[0], rayDirection, rayLength, wallLayerMask);

            if (hit.collider == null)
            {
                hit = Physics2D.CircleCast(pointPath[0], agent.EnclosingCircleRadius, rayDirection, rayLength, wallLayerMask);

                if (hit.collider == null)
                {
                    chosenRay = rayDirection * rayLength;

#if UNITY_EDITOR
                    gizmoChosenRay = chosenRay;
#endif

                    break;
                }
            }
        }

        if (chosenRay == null) return new SteeringGoal();

        goal.Position = agent.CenterPosition + (Vector2)chosenRay;
        return goal;
    }

    private void AdjustRayParameters(Vector2 origin, float agentRadius)
    {
        float safetyMargin = 0.001f;

        float shortestDistance = float.MaxValue;
        float angleStep = 360 / maxRayCount;
        float currentAngle = 0;

        for (int i = 0; i < maxRayCount; i++)
        {
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * pathDirection;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxRayLength, wallLayerMask);
            if (hit.collider != null && hit.distance < shortestDistance)
            {
                shortestDistance = hit.distance;
            }

            currentAngle += angleStep;
        }

        float minRayLength = agentRadius * margin;

        if (shortestDistance < minRayLength)
        {
            rayCount = maxRayCount;
            rayLength = minRayLength;
        }
        else
        {
            float distanceFraction = (maxRayLength - agentRadius) / (shortestDistance - agentRadius);
            rayCount = Mathf.RoundToInt(distanceFraction * maxRayCount);
            rayLength = shortestDistance + safetyMargin;
        }

        rayCount = maxRayCount;
        Debug.Log(rayCount);
        Debug.Log(rayLength);
    }

    private void OnDrawGizmos()
    {
        Agent agent = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>();
        Gizmos.color = Color.red;

        float angleStep = 360 / maxRayCount;
        float currentAngle = 0;

        for (int i = 0; i < maxRayCount; i++)
        {
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * pathDirection;
            Vector2 endPoint = agent.CenterPosition + rayDirection * maxRayLength;
            Gizmos.DrawLine(agent.CenterPosition, endPoint);
            currentAngle += angleStep;
        }

        if (rayCount > 0)
        {
            Gizmos.color = Color.yellow;
            angleStep = 360 / rayCount;
            currentAngle = 0;

            for (int i = 0; i < rayCount; i++)
            {
                Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * pathDirection;
                Vector2 endPoint = agent.CenterPosition + rayDirection * rayLength;
                Gizmos.DrawLine(agent.CenterPosition, endPoint);
                currentAngle += angleStep;
            }

        }

        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(agent.CenterPosition, agent.CenterPosition + gizmoChosenRay ?? Vector2.zero);
        Gizmos.DrawWireSphere(agent.CenterPosition, agent.EnclosingCircleRadius * margin);
    }
}