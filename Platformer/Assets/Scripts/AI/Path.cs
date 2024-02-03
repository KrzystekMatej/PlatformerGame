using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

[Serializable]
public class Path
{
    [field: NonSerialized]
    public List<Vector2> Points { get; private set; }
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
        public float SquaredDistance;
        public float ScalarProjection;

        public SegmentData(int index, float squaredDistance, float scalarProjection)
        {
            Index = index;
            SquaredDistance = squaredDistance;
            ScalarProjection = scalarProjection;
        }
    }

    public void SetPoints(List<Vector2> points)
    {
        if (points == null || points.Count <= 1)
        {
            throw new ArgumentException("Points list cannot be null or empty.", nameof(points));
        }

        if (this.Points == null)
        {
            this.Points = new List<Vector2>();
        }

        this.Points.Clear();
        this.Points.AddRange(points);
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
        if (isCircular) return MathUtility.GetCircularIndex(index, Points.Count);
        else return index;
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

        if (Mathf.Sqrt(closestSegment.SquaredDistance) >= radius) AddPathOffset(offset);
        else Target = null;
    }

    private void FindClosestSegment(Vector2 position, int startSegment, int endSegment)
    {
        closestSegment = new SegmentData(-1, float.MaxValue, 0);

        for (int i = startSegment; ; i = GetPathIndex(i + 1))
        {
            SegmentData segment = CalculateSegmentData(i, position);

            if (segment.SquaredDistance < closestSegment.SquaredDistance)
            {
                closestSegment = segment;
            }

            if (i == endSegment) break;
        }


        CalculateClosestPointIndex();
    }

    private void CalculateClosestPointIndex()
    {
        Vector2 startPoint = Points[closestSegment.Index];
        Vector2 endPoint = Points[GetPathIndex(closestSegment.Index + 1)];

        if (closestSegment.ScalarProjection <= Vector2.Distance(startPoint, endPoint) - closestSegment.ScalarProjection)
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

        float scalarProjection = MathUtility.GetScalarProjectionOnSegment(startPoint, endPoint, position);

       
        Vector2 normalPoint = startPoint + (endPoint - startPoint).normalized * scalarProjection;
        float squaredDistance = (position - normalPoint).sqrMagnitude;

        return new SegmentData(index, squaredDistance, scalarProjection);
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
        Gizmos.color = Color.white;
        int upperLimit = isCircular ? Points.Count : Points.Count - 1;
        for (int i = 0; i < upperLimit; i++)
        {
            a = Points[GetPathIndex(i)];
            b = Points[GetPathIndex(i + 1)];
            Gizmos.DrawLine(a, b);
        }

        Gizmos.color = Color.red;
        float pointRadius = 0.1f;
        if (Target != null)
        {
            Gizmos.DrawSphere((Vector2)Target, pointRadius);
        }

        Gizmos.color = Color.yellow;
        a = Points[closestSegment.Index];
        b = Points[GetPathIndex(closestSegment.Index + 1)];
        Vector2 direction = (b - a).normalized;
        b = a + direction * closestSegment.ScalarProjection;
        Gizmos.DrawLine(a, b);


        Gizmos.color = Color.yellow;
        a = b;
        b = currentPosition;
        Gizmos.DrawLine(a, b);
    }
}