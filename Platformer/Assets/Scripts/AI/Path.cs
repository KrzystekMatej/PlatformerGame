using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[Serializable]
public class Path
{
    public List<Vector2> Points { get; private set; } = new List<Vector2>();
    [SerializeField]
    private float pathOffset;
    [SerializeField]
    private float pathRadius;

    public Vector2 Target { get; private set; }
    private int closestPointIndex;
    private SegmentData closestSegment;

    private struct SegmentData
    {
        public int Index;
        public Vector2 Direction;
        public float Length;
        public float DistanceToSegment;
        public float ScalarProjection;

        public SegmentData(int index, Vector2 direction, float length, float distanceToSegment, float scalarProjection)
        {
            Index = index;
            Direction = direction;
            Length = length;
            DistanceToSegment = distanceToSegment;
            ScalarProjection = scalarProjection;
        }
    }

    public Path(List<Vector2> points)
    {
        Points = new List<Vector2>();
        SetPoints(points);
    }

    public void SetPoints(List<Vector2> points)
    {
        if (points is null || points.Count == 0)
        {
            throw new ArgumentException("Points list cannot be null or empty.", nameof(points));
        }

        if (Points == null)
        {
            Points = new List<Vector2>();
        }

        Points.Clear();
        Points.AddRange(points);
    }

    public void SetPoints(List<Transform> transforms)
    {
        if (transforms == null || transforms.Count == 0)
        {
            throw new ArgumentNullException("Points list cannot be null or empty.", nameof(transforms));
        }

        if (Points == null)
        {
            Points = new List<Vector2>();
        }

        if (Points.Count > transforms.Count)
        {
            Points.RemoveRange(transforms.Count, Points.Count - transforms.Count);
        }
        else while (Points.Count < transforms.Count)
        {
            Points.Add(Vector2.zero);
        }

        for (int i = 0; i < transforms.Count; i++)
        {
            Points[i] = new Vector2(transforms[i].position.x, transforms[i].position.y);
        }
    }

    public void CalculateTarget(Vector2 position)
    {
        FindTarget(position, 0, Points.Count - 1);
    }

    public void CalculateTargetNearLastPosition(Vector2 position)
    {
        int startSegment = Mathf.Max(closestPointIndex - 1, 0);
        int endSegment = Mathf.Min(closestPointIndex, Points.Count - 2);

        FindTarget(position, startSegment, endSegment);
    }

    private void FindTarget(Vector2 position, int startSegment,  int endSegment)
    {
        closestSegment = new SegmentData(-1, Vector2.zero, 0, float.MaxValue, 0);

        for (int i = startSegment; i <= endSegment; i++)
        {
            CalculateClosestSegmentData(i, position);
        }


        if (closestSegment.ScalarProjection <= closestSegment.Length - closestSegment.ScalarProjection)
        {
            closestPointIndex = closestSegment.Index;
        }
        else
        {
            closestPointIndex = closestSegment.Index + 1;
        }

        AddPathOffset(pathOffset);
    }

    private void CalculateClosestSegmentData(int index, Vector2 position)
    {
        Vector2 startPoint = Points[index % Points.Count];
        Vector2 endPoint = Points[(index + 1) % Points.Count];


        Vector2 positionDirection = position - startPoint;
        Vector2 segmentDirection = endPoint - startPoint;

        float segmentLength = segmentDirection.magnitude;
        float scalarProjection = 0;
        if (segmentLength != 0)
        {
            segmentDirection = segmentDirection / segmentLength;
            scalarProjection = Mathf.Clamp(Vector2.Dot(positionDirection, segmentDirection), 0, segmentLength);
        }

        Vector2 normalPoint = startPoint + segmentDirection * scalarProjection;
        float distanceToSegment = Vector2.Distance(position, normalPoint);


        if (distanceToSegment < closestSegment.DistanceToSegment)
        {
            closestSegment = new SegmentData(index, segmentDirection, segmentLength, distanceToSegment, scalarProjection);
        }
    }


    private void AddPathOffset(float pathOffset)
    {
        Vector2 startPoint = Points[closestSegment.Index % Points.Count];
        float targetDistance = pathOffset + closestSegment.ScalarProjection;

        if (targetDistance <= closestSegment.Length)
        {
            Target = startPoint + closestSegment.Direction * targetDistance;
        }
        else
        {
            float distanceOnNextSegment = targetDistance - closestSegment.Length;
            Vector2 midPoint = Points[(closestSegment.Index + 1) % Points.Count];

            if (closestSegment.Index + 2 > Points.Count - 1) Target = midPoint;
            else
            {
                Vector2 endPoint = Points[(closestSegment.Index + 2) % Points.Count];
                Vector2 nextSegmentDirection = (endPoint - midPoint).normalized;
                Target = midPoint + nextSegmentDirection * distanceOnNextSegment;
            }
        }
    }

    public void DrawGizmos(Vector2 currentPosition)
    {
        Vector2 a;
        Vector2 b;

        for (int i = 0; i < Points.Count; i++)
        {
            a = Points[i % Points.Count];
            b = Points[(i + 1) % Points.Count];
            Gizmos.color = Color.white;
            //Gizmos.DrawLine(a, b);
        }

        Gizmos.color = Color.red;
        float pointRadius = 0.1f;
        Gizmos.DrawSphere(Target, pointRadius);

        Gizmos.color = Color.yellow;
        a = Points[closestSegment.Index];
        b = a + closestSegment.Direction * closestSegment.ScalarProjection;
        Gizmos.DrawLine(a, b);


        Gizmos.color = Color.yellow;
        a = b;
        b = currentPosition;
        Gizmos.DrawLine(a, b);
    }
}