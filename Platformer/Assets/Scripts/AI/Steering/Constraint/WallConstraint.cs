using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class WallConstraint : AvoidConstraint, ISerializationCallbackReceiver
{
    [SerializeField]
    private float fanAngle;
    [SerializeField]
    private float centralRayLength;
    [SerializeField]
    private float sideRayLength;
    [SerializeField]
    private int rayCount;
    [SerializeField]
    private float margin = 1f;

    private RaycastHit2D closestHit;
    private Vector2 problemRay;

#if UNITY_EDITOR
    Vector2[] rays;
#endif

    private float middle;
    private float angleStep;
    private float lengthStep;

    public void OnBeforeSerialize() {}

    public void OnAfterDeserialize()
    {
        if (rayCount <= 0) throw new ArgumentException("RayCount has to be higher than 0.");
        if (centralRayLength < sideRayLength) throw new ArgumentException("CentralRayLength has to be longer then SideRayLength.");
        if (fanAngle != 0f && rayCount == 1) throw new ArgumentException("If RayCount is 1 then fan angle has to be 0.");

        if (rayCount == 1)
        {
            middle = 0;
            angleStep = 0;
            lengthStep = 0;
        }
        else
        {
            middle = (rayCount - 1) / 2f;
            angleStep = fanAngle / (rayCount - 1);
            lengthStep = (centralRayLength - sideRayLength) / middle;
        }

#if UNITY_EDITOR
        rays = new Vector2[rayCount];

        float currentAngle = -(fanAngle / 2f);

        for (int i = 0; i < rayCount; i++)
        {
            float rayLength = centralRayLength - Math.Abs(i - middle) * lengthStep;
            rays[i] = (Quaternion.Euler(0, 0, currentAngle) * Vector2.right).normalized * rayLength;
            currentAngle += angleStep;
        }
#endif
    }

    public override bool IsViolated(Agent agent, List<Vector2> pointPath)
    {
        Vector2 agentDirection = agent.RigidBody.velocity;
        float agentDirectionAngle = Mathf.Atan2(agentDirection.y, agentDirection.x) * Mathf.Rad2Deg;

        return CheckRays(agent.CenterPosition, agentDirectionAngle - (fanAngle / 2f));
    }

    private bool CheckRays(Vector2 origin, float startAngle)
    {
        bool pathBlocked = false;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < rayCount; i++)
        {
            float rayLength = centralRayLength - Math.Abs(i - middle) * lengthStep;
            Vector2 rayDirection = (Quaternion.Euler(0, 0, startAngle) * Vector3.right).normalized;

            hitCount = Physics2D.RaycastNonAlloc(origin, rayDirection, hits, rayLength, avoidLayerMask);
            pathBlocked |= hitCount > 0;

            for (int j = 0; j < hitCount; j++)
            {
                if (hits[j].distance < shortestDistance)
                {
                    shortestDistance = hits[j].distance;
                    closestHit = hits[j];
                    problemRay = rayDirection * rayLength;
                }
            }

#if UNITY_EDITOR
            rays[i] = rayDirection * rayLength;
#endif

            startAngle += angleStep;
        }

        return pathBlocked;
    }

    public override SteeringGoal Suggest(Agent agent, List<Vector2> pointPath, SteeringGoal goal)
    {
        Vector2 hitDirection = closestHit.point - agent.CenterPosition;
        Vector2 overShoot = problemRay - hitDirection;

        Vector2 wallVector = closestHit.normal.Perpendicular1();
        Vector2 agentDirection = agent.RigidBody.velocity;
        Vector2? collisionPoint = MathUtility.FindIntersection(closestHit.point, wallVector, agent.CenterPosition, agentDirection);

        if (collisionPoint == null)
        {
            return goal;
        }
        
        Vector2 target = collisionPoint.Value + closestHit.normal * overShoot.magnitude;

        pointPath.Clear();
        pointPath.Add(agent.CenterPosition);
        pointPath.Add(target);

        goal.Position = target;

        return goal;
    }

    private void OnDrawGizmos()
    {
        Agent agent = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>();
        Gizmos.color = Color.yellow;
        foreach (Vector2 ray in rays)
        {
            Vector2 endPoint = agent.CenterPosition + ray;
            Gizmos.DrawLine(agent.CenterPosition, endPoint);
        }

        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(closestHit.point, 0.3f);
    }
}
