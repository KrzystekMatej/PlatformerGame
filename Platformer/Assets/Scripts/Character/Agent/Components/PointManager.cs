using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointManager : MonoBehaviour
{

    public UnityEvent<int> OnSetPoints;

    private int points = 0;

    public void AddPoints(int value)
    {
        points += value;
        OnSetPoints?.Invoke(points);
    }
}
