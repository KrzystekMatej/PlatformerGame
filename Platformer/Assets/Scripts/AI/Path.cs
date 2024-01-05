using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Path
{
    public List<Vector2> Points { get; private set; }
    private List<float> pointParameters = new List<float>();

    public Path(List<Vector2> points)
    {
        SetPoints(points);
    }

    public void SetPoints(List<Vector2> points)
    {
        if (points is null || points.Count == 0)
        {
            throw new ArgumentException("Points list cannot be null or empty.", nameof(points));
        }

        Points = points;

        pointParameters.Add(0);

        for (int i = 1; i < Points.Count; i++)
        {
            pointParameters.Add(pointParameters[i - 1] + Vector2.Distance(points[i - 1], points[i]));
        }
    }

    public (float parameter, int segmentIndex) GetParameter(Vector2 position)
    {
        return FindParameter(position, 0, Points.Count - 1);
    }

    public (float parameter, int segmentIndex) GetCloseParameter(Vector2 position, int lastSegmentIndex)
    {
        return FindParameter(position, Mathf.Max(0, lastSegmentIndex - 1), Mathf.Min(lastSegmentIndex + 2));
    }

    private (float parameter, int segmentIndex) FindParameter(Vector2 position, int startSegment, int endSegment)
    {
        float closestDistance = float.MaxValue;
        float newParameter = 0;
        int newSegmentIndex = -1;

        for (int i = startSegment; i < endSegment; i++)
        {
            Vector2 startPoint = Points[i];
            Vector2 endPoint = Points[i + 1];


            Vector2 normalPoint = MathUtility.GetClosestPointOnSegment(startPoint, endPoint, position);
            float distanceToLine = Vector2.Distance(position, normalPoint);
            float scalarProjection = Vector2.Distance(startPoint, normalPoint);
            float currentParameter = pointParameters[i] + scalarProjection;

            if (distanceToLine < closestDistance)
            {
                closestDistance = distanceToLine;
                newParameter = currentParameter;
                newSegmentIndex = i;
            }
        }

        return (newParameter, newSegmentIndex);
    }

    public Vector2 GetPosition(float parameter)
    {
        int parameterIndex = FindClosestParameterIndex(parameter);
        return GetPosition(parameter, parameterIndex);
    }

    public Vector2 GetPosition(float parameter, int currentSegmentIndex)
    {
        parameter = Mathf.Clamp(parameter, 0, pointParameters[pointParameters.Count - 1]);
        currentSegmentIndex = Mathf.Clamp(currentSegmentIndex, 0, pointParameters.Count - 2);
        Vector2 segmentDirection = (Points[currentSegmentIndex + 1] - Points[currentSegmentIndex]).normalized;
        float segmentDistance = parameter - pointParameters[currentSegmentIndex];
        return Points[currentSegmentIndex] + segmentDirection * segmentDistance;
    }

    public int FindClosestParameterIndex(float parameter)
    {
        int left = 0;
        int right = pointParameters.Count - 1;
        while (left <= right)
        {
            int mid = (left + right) / 2;

            if (pointParameters[mid] < parameter)
            {
                left = mid + 1;
            }
            else if (pointParameters[mid] > parameter)
            {
                right = mid - 1;
            }
            else
            {
                return mid;
            }
        }

        return left > 0 ? left - 1 : 0;
    }
}