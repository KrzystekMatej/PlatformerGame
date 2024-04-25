using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VolumeComponent;

[Serializable]
public class Path
{
    [NonSerialized]
    public List<Vector2> Points = new List<Vector2>();
    [SerializeField]
    private float predictTime = 0.1f;
    [SerializeField]
    private float offset;
    [SerializeField]
    private float radius;
    [SerializeField]
    private bool isCircular;

#if UNITY_EDITOR
    private Vector2 gizmoFuturePosition;
    private Vector2 gizmoGoalPosition;
#endif

    private int closestPointIndex;
    private SegmentData closestSegment;

    private struct SegmentData
    {
        public int Index;
        public float SegmentLength;
        public float SquaredDistance;
        public float ScalarProjection;

        public SegmentData(int index, float segmentLength, float squaredDistance, float scalarProjection)
        {
            Index = index;
            SegmentLength = segmentLength;
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

    private int GetPointIndex(int index)
    {
        if (isCircular) return MathUtility.GetCircularIndex(index, Points.Count);
        return Mathf.Clamp(index, 0, Points.Count - 1);
    }

    private Vector2 GetFuturePosition(AgentManager agent)
    {
        return agent.PhysicsCenter + agent.RigidBody.velocity * predictTime;
    }

    public void Recalculate(AgentManager agent)
    {
        FindClosestSegment(GetFuturePosition(agent), 0, isCircular ? Points.Count - 1 : Points.Count - 2);
    }

    public Vector2 CalculateGoalWithoutCoherence(AgentManager agent)
    {
        Vector2 goalPosition = GetFuturePosition(agent);

        FindClosestSegment(GetFuturePosition(agent), 0, isCircular ? Points.Count - 1 : Points.Count - 2);

        if (Mathf.Sqrt(closestSegment.SquaredDistance) >= radius) goalPosition = GetOffsetGoal(offset);

#if UNITY_EDITOR
        gizmoGoalPosition = goalPosition;
#endif

        return goalPosition;
    }

    public Vector2 CalculateGoalWithCoherence(AgentManager agent)
    {
        Vector2 goalPosition = GetFuturePosition(agent);

        int ingoingSegment = isCircular ? GetPointIndex(closestPointIndex - 1) : Mathf.Max(closestPointIndex - 1, 0);
        int outgoingSegment = isCircular ? closestPointIndex : Mathf.Min(closestPointIndex, Points.Count - 2);

        FindClosestSegment(goalPosition, ingoingSegment, outgoingSegment);

        if (Mathf.Sqrt(closestSegment.SquaredDistance) >= radius || agent.RigidBody.velocity.magnitude < Mathf.Epsilon) goalPosition = GetOffsetGoal(offset);

#if UNITY_EDITOR
        gizmoGoalPosition = goalPosition;
#endif

        return goalPosition;
    }

    private void FindClosestSegment(Vector2 position, int startSegment, int endSegment)
    {
#if UNITY_EDITOR
        gizmoFuturePosition = position;
#endif

        closestSegment = new SegmentData(-1, 0, float.MaxValue, 0);

        for (int i = startSegment; ; i = GetPointIndex(i + 1))
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

    private SegmentData CalculateSegmentData(int index, Vector2 position)
    {
        Vector2 startPoint = Points[index];
        Vector2 endPoint = Points[GetPointIndex(index + 1)];

        Vector2 positionVector = position - startPoint;
        Vector2 segmentVector = endPoint - startPoint;

        float segmentLength = segmentVector.magnitude;
        float scalarProjection = 0;
        if (segmentLength != 0)
        {
            segmentVector = segmentVector / segmentLength;
            scalarProjection = Mathf.Clamp(Vector2.Dot(positionVector, segmentVector), 0, segmentLength);
        }

        Vector2 normalPoint = startPoint + segmentVector * scalarProjection;
        float squaredDistance = (position - normalPoint).sqrMagnitude;

        return new SegmentData(index, segmentLength, squaredDistance, scalarProjection);
    }

    private void CalculateClosestPointIndex()
    {
        Vector2 startPoint = Points[closestSegment.Index];
        Vector2 endPoint = Points[GetPointIndex(closestSegment.Index + 1)];

        if (closestSegment.ScalarProjection <= Vector2.Distance(startPoint, endPoint) - closestSegment.ScalarProjection)
        {
            closestPointIndex = closestSegment.Index;
        }
        else
        {
            closestPointIndex = GetPointIndex(closestSegment.Index + 1);
        }
    }

    private Vector2 GetOffsetGoal(float pathOffset)
    {
        float goalDistance = pathOffset + closestSegment.ScalarProjection;
        int direction = (int)Mathf.Sign(goalDistance);
        goalDistance = Mathf.Abs(goalDistance);

        for (int i = closestSegment.Index; isCircular || (direction == 1 && i < Points.Count - 1) || (direction == -1 && i > 0); i = GetPointIndex(i + direction))
        {
            Vector2 startPoint = Points[i];
            Vector2 endPoint = Points[GetPointIndex(i + direction)];

            float segmentLength = Vector2.Distance(startPoint, endPoint);

            if (goalDistance <= segmentLength)
            {
                Vector2 segmentDirection = (endPoint - startPoint).normalized;
                return startPoint + segmentDirection * goalDistance;
            }

            goalDistance -= segmentLength;
        }

        return Points[direction == 1 ? Points.Count - 1 : 0];
    }

    public void SmoothOut(float agentRadius, LayerMask solidGeometryLayerMask)
    {
        for (int i = 0; i < Points.Count - 2; i++)
        {
            Vector2 edgeVector = Points[i + 2] - Points[i];
            if (!Physics2D.CircleCast(Points[i], agentRadius, edgeVector, edgeVector.magnitude, solidGeometryLayerMask))
            {
                Points.RemoveAt(i + 1);
            }
        }
    }

    public bool ReachedEnd(AgentManager agent, Vector2 goal)
    {
        return !isCircular && goal == Points[Points.Count - 1] && Vector2.Distance(agent.PhysicsCenter, goal) <= agent.PhysicsRadius;
    }

#if UNITY_EDITOR
    public void DrawPathGizmos()
    {
        Gizmos.color = Color.white;
        int upperLimit = isCircular ? Points.Count : Points.Count - 1;
        for (int i = 0; i < upperLimit; i++)
        {
            Vector2 a = Points[GetPointIndex(i)];
            Vector2 b = Points[GetPointIndex(i + 1)];
            Gizmos.DrawLine(a, b);
        }
    }

    public void DrawGoalGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 a = Points[closestSegment.Index];
        Vector2 b = Points[GetPointIndex(closestSegment.Index + 1)];
        Vector2 direction = (b - a).normalized;
        b = a + direction * closestSegment.ScalarProjection;
        Gizmos.DrawLine(a, b);

        Gizmos.color = Color.yellow;
        a = b;
        b = gizmoFuturePosition;
        Gizmos.DrawLine(a, b);

        Gizmos.color = Color.red;
        float pointRadius = 0.1f;
        Gizmos.DrawSphere(gizmoGoalPosition, pointRadius);
    }

    public void DrawAllGizmos()
    {
        if (Points.Count < 2) return;
        DrawPathGizmos();
        DrawGoalGizmos();
    }
#endif
}