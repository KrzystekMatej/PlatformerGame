using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Path
{
    [field: SerializeField]
    public List<Transform> Points { get; private set; }
    private List<float> pointParameters = new List<float>();

    public void Initialize()
    {
        if (Points is null || Points.Count == 0)
        {
            throw new ArgumentException("Points list cannot be null or empty.", nameof(Points));
        }

        pointParameters.Add(0);

        for (int i = 1; i < Points.Count; i++)
        {
            pointParameters.Add(pointParameters[i - 1] + Vector2.Distance(Points[i - 1].position, Points[i].position));
        }
    }

    public (float parameter, int segmentIndex) GetParameter(Vector2 position)
    {
        return FindParameter(position, 0, Points.Count - 2);
    }

    public (float parameter, int segmentIndex) GetCloseParameter(Vector2 position, int lastClosestPointIndex)
    {
        int startSegment = Mathf.Max(0, lastClosestPointIndex - 1);
        int endSegment = Mathf.Min(lastClosestPointIndex, Points.Count - 2);

        return FindParameter(position, startSegment, endSegment);
    }

    private (float parameter, int segmentIndex) FindParameter(Vector2 position, int startSegment, int endSegment)
    {
        float closestDistance = float.MaxValue;
        float newParameter = 0;
        int newClosestPointIndex = -1;

        for (int i = startSegment; i <= endSegment; i++)
        {
            Vector2 startPoint = Points[i].position;
            Vector2 endPoint = Points[i + 1].position;


            Vector2 normalPoint = MathUtility.GetClosestPointOnSegment(startPoint, endPoint, position);
            float distanceToLine = Vector2.Distance(position, normalPoint);
            float scalarProjection = Vector2.Distance(startPoint, normalPoint);
            float currentParameter = pointParameters[i] + scalarProjection;

            if (distanceToLine < closestDistance)
            {
                closestDistance = distanceToLine;
                newParameter = currentParameter;

                float segmentDistance = pointParameters[i + 1] - pointParameters[i];
                newClosestPointIndex = scalarProjection < segmentDistance - scalarProjection ? i : i + 1;
            }
        }

        return (newParameter, newClosestPointIndex);
    }

    public Vector2 GetPosition(float parameter)
    {
        int parameterIndex = FindClosestParameterIndex(parameter);
        return GetPosition(parameter, parameterIndex);
    }

    public Vector2 GetPosition(float parameter, int closestPointIndex)
    {
        parameter = Mathf.Clamp(parameter, 0, pointParameters[pointParameters.Count - 1]);

        int currentSegmentIndex = Mathf.Clamp(closestPointIndex, 0, pointParameters.Count - 2);
        Vector2 segmentDirection = (Points[currentSegmentIndex + 1].position - Points[currentSegmentIndex].position).normalized;
        float distanceToParameter = parameter - pointParameters[currentSegmentIndex];
        return (Vector2)Points[currentSegmentIndex].position + segmentDirection * distanceToParameter;
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