using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class MathUtility
{
    public const int recommendedFreeCollisionCount = 16;

    public static Vector2 PolarCoordinatesToVector2(float angleRad, float magnitude)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * magnitude;
    }

    public static Vector2 GetSignedVector(Vector2 vector)
    {
        return new Vector2(Math.Sign(vector.x), Math.Sign(vector.y));
    }

    public static Vector2 GetAbsoluteVector(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    public static Vector2 PerpendicularCC(Vector2 vector)
    {
        return new Vector2(vector.y, -vector.x);
    }

    public static Vector2 PerpendicularC(Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }


    public static Vector2 GetExpansionOffsetCC(Vector2 ingoing, Vector2 outgoing, float radius)
    {
        if (ingoing == Vector2.zero && outgoing == Vector2.zero) return Vector2.zero;
        Vector2 normal1 = PerpendicularCC(ingoing);
        Vector2 normal2 = PerpendicularCC(outgoing);

        Vector2 offsetPoint1 = normal1 * radius;
        Vector2 offsetPoint2 = normal2 * radius;

        return (Vector2)FindLineLineIntersection(offsetPoint1, ingoing, offsetPoint2, outgoing);
    }

    public static Vector2 CalculateStopOffset(Vector2 currentVelocity, float maxForce)
    {
        return -new Vector2(CalculateStopDistance(currentVelocity.x, maxForce), CalculateStopDistance(currentVelocity.y, maxForce));
    }

    public static float CalculateStopDistance(float currentSpeed, float maxForce)
    {
        return currentSpeed * currentSpeed / (maxForce * 2);
    }

    public static float CalculateTimeToStop(float currentSpeed, float maxForce)
    {
        return currentSpeed / maxForce;
    }

    public static float GetVectorRadAngle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static float GetRandomBinomial()
    {
        return UnityEngine.Random.value - UnityEngine.Random.value;
    }

    public static int GetCircularIndex(int index, int length)
    {
        int qZeroRounding = index / length;
        int roundingCorrection = ((index ^ length) < 0) && (index % length != 0) ? 1 : 0;
        int qNegInfRounding = qZeroRounding - roundingCorrection;
        int r = index - length * qNegInfRounding;
        return r;
    }

    public static bool IsAngleConvexCC(Vector3 a, Vector3 b)
    {
        return Vector3.Cross(a, b).z >= 0;
    }

    public static float GetScalarProjectionOnSegment(Vector2 startPoint, Vector2 endPoint, Vector2 position)
    {
        Vector2 u = position - startPoint;
        Vector2 v = endPoint - startPoint;

        float segmentLength = v.magnitude;
        if (segmentLength == 0) return 0;

        Vector2 n = v / segmentLength;

        return Mathf.Clamp(Vector2.Dot(u, n), 0, segmentLength);
    }

    public static int FindLineCircleIntersections(Vector2 point, Vector2 direction, Vector2 center, float radius, Vector2[] intersections)
    {
        float a = direction.x * direction.x + direction.y * direction.y;
        float b = 2 * (point.x * direction.x - direction.x * center.x + point.y * direction.y - direction.y * center.y);
        float c = point.x * point.x + point.y * point.y + center.x * center.x + center.y * center.y - radius * radius - 2 * (point.x * center.x + point.y * center.y);
        float d = b * b - 4 * a * c;

        if (d < 0) return 0;
        else if (Mathf.Abs(d) < Mathf.Epsilon && intersections.Length >= 1)
        {
            float t = -b / (2 * a);
            intersections[0] = point + direction * t;
            return 1;
        }
        else if (intersections.Length >= 2)
        {
            float root = Mathf.Sqrt(d);
            float t1 = (-b + root) / (2 * a);
            float t2 = (-b - root) / (2 * a);

            if (t1 < t2)
            {
                intersections[0] = point + direction * t1;
                intersections[1] = point + direction * t2;
            }
            else
            {
                intersections[1] = point + direction * t1;
                intersections[0] = point + direction * t2;
            }
            return 2;
        }

        throw new ArgumentException("The array is too small to hold all intersections.", nameof(intersections));
    }

    public static Vector2? FindLineLineIntersection(Vector2 a, Vector2 u, Vector2 b, Vector2 v)
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

    public static float GetSignedArea(Vector2[] points)
    {
        float area = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[(i + 1) % points.Length];

            area += a.x * b.y - b.x * a.y;
        }

        return area / 2;
    }

    public static bool IsPointInsideCompositeCollider(Vector2 point, CompositeCollider2D collider)
    {
        for (int i = 0; i < collider.pathCount; i++)
        {
            Vector2[] pathPoints = new Vector2[collider.GetPathPointCount(i)];
            collider.GetPath(i, pathPoints);

            if (IsPointInsidePolygon(point, pathPoints))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPointInsidePolygon(Vector2 point, Vector2[] polygonPoints)
    {
        int intersections = 0;
        for (int i = 0; i < polygonPoints.Length; i++)
        {
            Vector2 a = polygonPoints[i];
            Vector2 b = polygonPoints[(i + 1) % polygonPoints.Length];

            if ((a.y > point.y) != (b.y > point.y))
            {
                float intersectX = (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x;

                if (point.x < intersectX)
                {
                    intersections++;
                }
            }
        }

        return (intersections % 2) == 1;
    }

    public static float GetEnclosingCircleRadius(Collider2D collider)
    {
        if (collider is CircleCollider2D circleCollider) return circleCollider.radius;
        else if (collider is CapsuleCollider2D capsuleCollider)
        {
            if (capsuleCollider.direction == CapsuleDirection2D.Vertical) return capsuleCollider.size.y / 2;
            return capsuleCollider.size.x / 2;
        }

        float diagonal = Mathf.Sqrt(collider.bounds.size.x * collider.bounds.size.x + collider.bounds.size.y * collider.bounds.size.y);
        return diagonal / 2;
    }

    public static Vector2? GetCollisionFreePosition(Vector2 position, float agentRadius, LayerMask collisionMask)
    {
        const float safetyMargin = 0.001f;
        for (int i = 0; i < recommendedFreeCollisionCount; i++)
        {
            RaycastHit2D hit = Physics2D.CircleCast(position, agentRadius, Vector2.zero, 0, collisionMask);
            if (hit)
            {
                Vector2 direction = (hit.point - position).normalized;
                hit = Physics2D.Raycast(position, direction, agentRadius, collisionMask);
                position = position - direction * (safetyMargin + agentRadius - hit.distance);
            }
            else return position;
        }

        return null;
    }

    public static Vector2[] GetBoxColliderPath(BoxCollider2D collider)
    {
        float top = collider.offset.y + collider.size.y / 2;
        float bottom = collider.offset.y - collider.size.y / 2;
        float right = collider.offset.x + collider.size.y / 2;
        float left = collider.offset.x - collider.size.y / 2;

        Vector2[] path = new Vector2[]
        {
            new Vector2(right, top),
            new Vector2(left, top),
            new Vector2(left, bottom),
            new Vector2(right, bottom)
        };

        return path
            .Select(p => (Vector2)collider.transform.TransformPoint(p))
            .ToArray();
    }

    public static Vector2[] GetCircleColliderInscribedPath(CircleCollider2D collider, int pointCount)
    {
        return GetRegularPolygonPath(collider.radius, pointCount)
            ?.Select(p => (Vector2)collider.transform.TransformPoint(p + collider.offset))
            .ToArray();
    }

    public static Vector2[] GetCircleColliderCircumscribedPath(CircleCollider2D collider, int pointCount)
    {
        float radius = collider.radius * (1f / Mathf.Cos(Mathf.PI/pointCount));

        return GetRegularPolygonPath(radius, pointCount)
            ?.Select(p => (Vector2)collider.transform.TransformPoint(p + collider.offset))
            .ToArray();
    }

    public static Vector2[] GetRegularPolygonPath(float radius, int pointCount)
    {
        if (pointCount < 3) return null;

        float angleStep = 2 * Mathf.PI / pointCount;
        Vector2[] points = new Vector2[pointCount];
        float angle = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            points[i] = new Vector2(x, y);
            angle += angleStep;
        }

        return points;
    }

    public static List<Vector2[]> GetPolygonColliderPaths(PolygonCollider2D collider)
    { 
        List<Vector2[]> paths = new List<Vector2[]>();
        for (int i = 0; i < collider.pathCount; i++)
        {
            Vector2[] points = collider
                .GetPath(i)
                .Select(p => (Vector2)collider.transform.TransformPoint(p + collider.offset))
                .ToArray();
            paths.Add(points);
        }

        return paths;
    }

    public static List<Vector2[]> GetCompositeColliderPaths(CompositeCollider2D collider)
    {
        List<Vector2[]> paths = new List<Vector2[]>();
        for (int i = 0; i < collider.pathCount; i++)
        {
            Vector2[] points = new Vector2[collider.GetPathPointCount(i)];
            collider.GetPath(i, points);
            points = points
                .Select(p => (Vector2)collider.transform.TransformPoint(p + collider.offset))
                .ToArray();
            paths.Add(points);
        }

        return paths;
    }

    public static List<Vector2[]> GetColliderPaths(Collider2D collider, float circlePathRatio)
    {
        if (collider is BoxCollider2D box)
        {
            return new List<Vector2[]> { GetBoxColliderPath(box) };
        }
        else if (collider is CircleCollider2D circle)
        {
            return new List<Vector2[]> { GetCircleColliderCircumscribedPath(circle, (int)Mathf.Round(circle.radius * circlePathRatio)) };
        }
        else if (collider is PolygonCollider2D polygon)
        {
            return GetPolygonColliderPaths(polygon);
        }
        else if (collider is EdgeCollider2D edge)
        {
            return new List<Vector2[]> { edge.points };
        }
        else if (collider is CompositeCollider2D composite)
        {
            return GetCompositeColliderPaths(composite);
        }
        else
        {
            throw new ArgumentException("Not allowed collider type.");
        }
    }

    public static Vector2[] GetDirectionsInCircle(int resolution)
    {
        float angleStep = 360 / resolution;
        float angle = 0;
        Vector2[] directions = new Vector2[resolution];
        for (int i = 0; i < resolution; i++)
        {
            directions[i] = Quaternion.Euler(0, 0, angle) * Vector2.right;
            angle += angleStep;
        }

        return directions;
    }
}