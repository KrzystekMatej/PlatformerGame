using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

    public static float GetRandomBinomial()
    {
        return UnityEngine.Random.value - UnityEngine.Random.value;
    }

    public static float GetNormalizedAngle(Vector2 vector)
    {
        float angleRadians = Mathf.Atan2(vector.y, vector.x);
        float normalizedAngle = angleRadians / Mathf.PI;

        return normalizedAngle;
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

    public static float GetVectorRadAngle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static int GetCircularIndex(int index, int length)
    {
        int qZeroRounding = index / length;
        int roundingCorrection = ((index ^ length) < 0) && (index % length != 0) ? 1 : 0;
        int qNegInfRounding = qZeroRounding - roundingCorrection;
        int r = index - length * qNegInfRounding;
        return r;
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

    public static bool IsAngleConvexCC(Vector3 a, Vector3 b)
    {
        return Vector3.Cross(a, b).z >= 0;
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

    public static Vector2 CalculateStopOffset(Vector2 currentVelocity, float maxForce)
    {
        return - new Vector2(CalculateStopDistance(currentVelocity.x, maxForce), CalculateStopDistance(currentVelocity.y, maxForce));
    }

    public static float CalculateStopDistance(float currentSpeed, float maxForce)
    {
        return currentSpeed * currentSpeed / (maxForce * 2);
    }

    public static float CalculateTimeToStop(float currentSpeed, float maxForce)
    {
        return currentSpeed / maxForce;
    }
}