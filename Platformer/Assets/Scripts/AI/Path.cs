using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Path
{
    public List<Vector2> Points { get; private set; } = new List<Vector2>();
    [SerializeField]
    private float offset;
    [SerializeField]
    private float radius;
    [SerializeField]
    private bool isCircular;

    public Vector2? Target { get; private set; }
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
        if (points == null || points.Count <= 1)
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
        if (transforms == null || transforms.Count <= 1)
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

    private int GetPathIndex(int index)
    {
        if (isCircular)
        {
            int count = Points.Count;
            int qZeroRounding = index / count;
            int roundingCorrection = ((index ^ count) < 0) && (index % count != 0) ? 1 : 0;
            int qNegInfRounding = qZeroRounding - roundingCorrection;


            int r = index - count * qNegInfRounding;
            return r;
        }
        return index;
    }

    public void Recalculate(Vector2 position)
    {
        FindClosestSegment(position, 0, isCircular ? Points.Count - 1 : Points.Count - 2);
    }

    public void CalculateTarget(Vector2 position)
    {
        int ingoingSegment = isCircular ? GetPathIndex(closestPointIndex - 1) : Mathf.Max(closestPointIndex - 1, 0);
        int outgoingSegment = isCircular ? closestPointIndex : Mathf.Min(closestPointIndex, Points.Count - 2);

        FindClosestSegment(position, ingoingSegment, outgoingSegment);

        if (closestSegment.DistanceToSegment > radius) AddPathOffset(offset);
        else Target = null;
    }

    private void FindClosestSegment(Vector2 position, int startSegment, int endSegment)
    {
        closestSegment = new SegmentData(-1, Vector2.zero, 0, float.MaxValue, 0);

        for (int i = startSegment; ; i = GetPathIndex(i + 1))
        {
            SegmentData segment = CalculateSegmentData(i, position);

            if (segment.DistanceToSegment < closestSegment.DistanceToSegment)
            {
                closestSegment = segment;
            }

            if (i == endSegment) break;
        }


        CalculateClosestPointIndex();
    }

    private void CalculateClosestPointIndex()
    {
        if (closestSegment.ScalarProjection <= closestSegment.Length - closestSegment.ScalarProjection)
        {
            closestPointIndex = closestSegment.Index;
        }
        else
        {
            closestPointIndex = GetPathIndex(closestSegment.Index + 1);
        }
    }

    private SegmentData CalculateSegmentData(int index, Vector2 position)
    {
        Vector2 startPoint = Points[index];
        Vector2 endPoint = Points[GetPathIndex(index + 1)];


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

        return new SegmentData(index, segmentDirection, segmentLength, distanceToSegment, scalarProjection);
    }


    private void AddPathOffset(float pathOffset)
    {
        float targetDistance = pathOffset + closestSegment.ScalarProjection;
        int direction = (int)Mathf.Sign(targetDistance);
        targetDistance = Mathf.Abs(targetDistance);

        for (int i = closestSegment.Index; isCircular || (direction == 1 && i < Points.Count - 1) || (direction == -1 && i > 0); i = GetPathIndex(i + direction))
        {
            Vector2 startPoint = Points[i];
            Vector2 endPoint = Points[GetPathIndex(i + direction)];

            float segmentLength = Vector2.Distance(startPoint, endPoint);

            if (targetDistance <= segmentLength)
            {
                Vector2 segmentDirection = (endPoint - startPoint).normalized;
                Target = startPoint + segmentDirection * targetDistance;
                return;
            }

            targetDistance -= segmentLength;
        }

        Target = Points[direction == 1 ? Points.Count - 1 : 0];
    }

    public void DrawGizmos(Vector2 currentPosition)
    {
        Vector2 a;
        Vector2 b;

        int upperLimit = isCircular ? Points.Count : Points.Count - 1;
        for (int i = 0; i < upperLimit; i++)
        {
            a = Points[GetPathIndex(i)];
            b = Points[GetPathIndex(i + 1)];
            Gizmos.color = Color.white;
            Gizmos.DrawLine(a, b);
        }

        Gizmos.color = Color.red;
        float pointRadius = 0.1f;
        if (Target != null)
        {
            Gizmos.DrawSphere((Vector2)Target, pointRadius);
        }

        Gizmos.color = Color.yellow;
        a = Points[GetPathIndex(closestSegment.Index)];
        b = a + closestSegment.Direction * closestSegment.ScalarProjection;
        Gizmos.DrawLine(a, b);


        Gizmos.color = Color.yellow;
        a = b;
        b = currentPosition;
        Gizmos.DrawLine(a, b);
    }
}