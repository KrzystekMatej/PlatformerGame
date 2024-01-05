using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static Vector2 GetClosestPointOnSegment(Vector2 startPoint, Vector2 endPoint, Vector2 position)
    {
        Vector2 u = position - startPoint;
        Vector2 v = endPoint - startPoint;

        float lineSegmentMagnitude = v.magnitude;
        if (lineSegmentMagnitude == 0) return startPoint;

        Vector2 n = v / lineSegmentMagnitude;
        
        return startPoint + n * Mathf.Clamp(Vector2.Dot(u, v), 0, lineSegmentMagnitude);
    }
}
