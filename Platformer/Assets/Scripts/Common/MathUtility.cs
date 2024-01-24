using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static float GetScalarProjectionOnSegment(Vector2 startPoint, Vector2 endPoint, Vector2 position)
    {
        Vector2 u = position - startPoint;
        Vector2 v = endPoint - startPoint;

        float segmentLength = v.magnitude;
        if (segmentLength == 0) return 0;

        Vector2 n = v / segmentLength;
        
        return Mathf.Clamp(Vector2.Dot(u, n), 0, segmentLength);
    }

    public static Vector2? FindIntersection(Vector2 a, Vector2 u, Vector2 b, Vector2 v)
    {
        float t;
        float denominator = u.x * v.y - u.y * v.x;

        if (Mathf.Abs(denominator) < Mathf.Epsilon)
        {
            return null;
        }

        t = ((b.x - a.x) * v.y - (b.y - a.y) * v.x) / denominator;
        return a + t * u;
    }

    public static float GetEnclosingCircleRadius(Collider2D collider)
    {
        if (collider is CapsuleCollider2D capsuleCollider)
        {
            Vector2 boxSize;
            if (capsuleCollider.direction == CapsuleDirection2D.Vertical)
            {
                boxSize = new Vector2(2 * capsuleCollider.size.x, capsuleCollider.size.y);
            }
            else
            {
                boxSize = new Vector2(capsuleCollider.size.x, 2 * capsuleCollider.size.y);
            }
            float diagonal = Mathf.Sqrt(boxSize.x * boxSize.x + boxSize.y * boxSize.y);
            return diagonal / 2;
        }
        else if (collider is BoxCollider2D boxCollider)
        {
            float diagonal = Mathf.Sqrt(boxCollider.size.x * boxCollider.size.x + boxCollider.size.y * boxCollider.size.y);
            return diagonal / 2;
        }
        else if (collider is CircleCollider2D circleCollider)
        {
            return circleCollider.radius;
        }

        throw new ArgumentException($"Collider of {collider.GetType()} type is not allowed for circle approximation.");
    }
}
