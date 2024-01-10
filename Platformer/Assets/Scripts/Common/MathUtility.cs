using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility 
{
    public static float GetScalarProjectionOnSegment(Vector2 startPoint, Vector2 endPoint, Vector2 position)
    {
        Vector2 u = position - startPoint;
        Vector2 v = endPoint - startPoint;

        float lineSegmentMagnitude = v.magnitude;
        if (lineSegmentMagnitude == 0) return 0;

        Vector2 n = v / lineSegmentMagnitude;
        
        return Mathf.Clamp(Vector2.Dot(u, n), 0, lineSegmentMagnitude);
    }
}
