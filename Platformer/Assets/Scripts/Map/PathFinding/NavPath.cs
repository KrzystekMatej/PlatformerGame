using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NavPath : IComparable<NavPath>
{
    [SerializeField]
    public List<NavGraphNode> Nodes;
    [SerializeField]
    public float Length;

    public NavPath()
    {
        Nodes = new List<NavGraphNode>();
    }

    public NavPath(List<NavGraphNode> path, float length)
    {
        Nodes = path;
        Length = length;
    }

    public NavGraphNode GetStart()
    {
        return Nodes[0];
    }

    public NavGraphNode GetEnd()
    {
        return Nodes[Nodes.Count - 1];
    }

    public int CompareTo(NavPath other)
    {
        int lengthComparison = Length.CompareTo(other.Length);
        if (lengthComparison != 0) return lengthComparison;

        int countComparison = Nodes.Count.CompareTo(other.Nodes.Count);
        if (countComparison != 0) return countComparison;

        for (int i = 0; i < Nodes.Count; i++)
        {
            Vector3 a = Nodes[i].transform.position;
            Vector3 b = other.Nodes[i].transform.position;

            int xComparison = a.x.CompareTo(b.x);
            if (xComparison != 0) return xComparison;

            int yComparison = a.y.CompareTo(b.y);
            if (yComparison != 0) return yComparison;
        }

        return 0;
    }

    public IEnumerable<Vector2> GetExpandedPositions(float radius, int start, int count)
    {
        for (int i = start; i < count; i++)
        {
            yield return Nodes[i].GetExpandedPosition(radius);
        }
    }
}