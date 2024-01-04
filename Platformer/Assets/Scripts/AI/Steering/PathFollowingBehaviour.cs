using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingBehaviour : SeekBehaviour
{
    [SerializeField]
    private List<Transform> path;
    [SerializeField]
    private float predictTime = 0.1f;
    [SerializeField]
    private float worldRecord = 1000000;
    [SerializeField]
    private float pathRadius = 10f;


    public override Vector2 GetSteering(Agent agent, Vision vision)
    {
        Vector2 futurePosition = (Vector2)transform.position + agent.RigidBody.velocity * predictTime;
        Vector2 normal = Vector2.zero;
        Vector2 target = Vector2.zero;


        for (int i = 0; i < path.Count; i++)
        {
            Vector2 a = path[i].position;
            Vector2 b = path[(i + 1) % path.Count].position;

            Vector2 normalPoint = GetNormalPoint(futurePosition, a, b);

            Vector2 direction = a - b;
            if
            (
                normalPoint.x < Mathf.Min(a.x, b.x) ||
                normalPoint.x > Mathf.Max(a.x, b.x) ||
                normalPoint.y < Mathf.Min(a.y, b.y) ||
                normalPoint.y > Mathf.Max(a.y, b.y)
            )
            {
                normalPoint = b;
                a = path[(i + 1) % path.Count].position;
                b = path[(i + 2) % path.Count].position;
                direction = b - a;
            }

            float d = Vector2.Distance(futurePosition, normalPoint);
            if (d < worldRecord)
            {
                worldRecord = d;
                normal = normalPoint;

                target = normal + direction.normalized * predictTime;
            }
        }

        if (worldRecord > pathRadius)
        {
            return CalculateSteeringForce(agent, target);
        }
        else
        {
            return Vector2.zero;
        }
    }


    private Vector2 GetNormalPoint(Vector2 futurePosition, Vector2 a, Vector2 b)
    {
        Vector2 af = futurePosition - a;
        Vector2 ab = (b - a).normalized;
        return a + ab * Vector2.Dot(af, ab);
    }
}
