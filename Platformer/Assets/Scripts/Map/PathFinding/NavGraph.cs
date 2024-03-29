using FibonacciHeap;
using System;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Unity.VisualScripting.Member;

public class NavGraph : MonoBehaviour
{
    public LayerMask WallMask;
    [SerializeField]
    private bool precalculationEnabled;
    public float CollisionAvoidanceMargin = 0.1f;
    public float MaxAllowedRadius = 0.9f;
    public float CirclePathRatio = 0.5f;

#if UNITY_EDITOR
    public Sprite NodeSprite;
    public List<Collider2D> TraversableZones;
    public List<Collider2D> Walls;
    [HideInInspector]
    public GameObject NodeContainer;

    public bool ShowEdges;
    public NavPath TestPath;
#endif

    [HideInInspector]
    public List<NavGraphNode> Nodes = new List<NavGraphNode>();
    private ulong searchId;
    private (int nodeIndex, float length)[,] shortestPathTable;

    private void Awake()
    {
        if (precalculationEnabled) PrecalculateShortestPaths();
    }

    public void AddNode(NavGraphNode nodeA)
    {
        nodeA.Index = Nodes.Count;
        Nodes.Add(nodeA);
    }

    public void RemoveNode(NavGraphNode nodeA)
    {

        foreach (NavGraphNode nodeB in Nodes)
        {
            nodeB.RemoveUndirectNeighbor(nodeA);
        }

        Nodes.RemoveAt(nodeA.Index);

        for (int i = nodeA.Index; i < Nodes.Count; i++)
        {
            Nodes[i].Index = i;
        }
    }

    public void AddEdges(NavGraphNode nodeA, float radius)
    {
        foreach (NavGraphNode nodeB in Nodes)
        {
            Vector2 positionA = nodeA.GetExpandedPosition(radius);
            Vector2 positionB = nodeB.GetExpandedPosition(radius);
            Vector2 edgeVector = positionB - positionA;
            RaycastHit2D hit = Physics2D.CircleCast(positionA, radius, edgeVector, edgeVector.magnitude, WallMask);

            if (!hit)
            {
                nodeA.AddUndirectNeighbor(nodeB);
            }
        }
    }


    public NavPath DijkstraShortestPath(NavGraphNode start, Func<NavGraphNode, bool> terminationCondition)
    {
        return FindShortestPath
        (
            start,
            terminationCondition,
            (current) => 0
        );
    }

    public NavPath FindShortestPath(NavGraphNode start, NavGraphNode end)
    {
        if (precalculationEnabled) return GetPrecalculatedPath(start, end);
        return AStarShortestPath(start, end);
    }

    public NavPath AStarShortestPath(NavGraphNode start, NavGraphNode end)
    {
        return FindShortestPath
        (
            start,
            (current) => current == end,
            (current) => Vector2.Distance(current.transform.position, end.transform.position)
        );
    }

    public NavPath FindShortestPath(NavGraphNode start, Func<NavGraphNode, bool> terminationCondition, Func<NavGraphNode, float> heuristic)
    {
        IncreaseSearchId();

        FibonacciHeap<NavGraphNode, float> heap = new FibonacciHeap<NavGraphNode, float>(float.NegativeInfinity);

        start.SearchId = searchId;
        start.GCost = 0;
        start.HCost = heuristic(start);
        start.HeapNode = new FibonacciHeapNode<NavGraphNode, float>(start, start.HCost);
        start.Closed = false;
        start.From = null;
        heap.Insert(start.HeapNode);

        while (!heap.IsEmpty())
        {
            NavGraphNode currentNode = heap.Min().Data;

            if (terminationCondition(currentNode)) return ReconstructPath(currentNode);

            foreach (NavGraphNode neighbor in currentNode.Neighbors)
            {
                float neighborGCost = currentNode.GCost + Vector2.Distance(currentNode.transform.position, neighbor.transform.position);

                if (neighbor.SearchId == searchId)
                {
                    if (neighbor.Closed) continue;

                    if (neighborGCost < neighbor.GCost) heap.DecreaseKey(neighbor.HeapNode, neighborGCost + neighbor.HCost);
                    else continue;
                }
                else
                {
                    neighbor.SearchId = searchId;
                    neighbor.HCost = heuristic(neighbor);
                    neighbor.Closed = false;
                    neighbor.HeapNode = new FibonacciHeapNode<NavGraphNode, float>(neighbor, neighborGCost + neighbor.HCost);
                    heap.Insert(neighbor.HeapNode);
                }

                neighbor.GCost = neighborGCost;
                neighbor.From = currentNode;
            }

            heap.RemoveMin();
            currentNode.Closed = true;
        }

        return null;
    }

    public List<NavPath> YenKShortestPaths(NavGraphNode start, NavGraphNode end, int k) => YenShortestPaths(start, end, (i, paths) => i <= k);

    public List<NavPath> YenShortestPaths(NavGraphNode start, NavGraphNode end, Func<int, List<NavPath>, bool> terminationCondition)
    {
        var shortestPathData = AStarShortestPath(start, end);
        if (shortestPathData.Nodes == null) return null;

        List<NavPath> kShortestPaths = new List<NavPath>() { shortestPathData };
        SortedSet<NavPath> set = new SortedSet<NavPath>();

        for (int k = 1; terminationCondition(k, kShortestPaths); k++)
        {
            for (int i = 0; i <= kShortestPaths[k - 1].Nodes.Count - 2; i++)
            {
                NavGraphNode spur = kShortestPaths[k - 1].Nodes[i];
                List<NavGraphNode> rootPath = kShortestPaths[k - 1].Nodes.GetRange(0, i + 1);

                List<int> relatedPathIndexes = new List<int>();
                for (int j = 0; j < kShortestPaths.Count; j++)
                {
                    List<NavGraphNode> relatedPath = kShortestPaths[j].Nodes;
                    if (relatedPath.Count < i + 1) continue;
                    if (rootPath.SequenceEqual(relatedPath.GetRange(0, i + 1)))
                    {
                        relatedPath[i].RemoveUndirectNeighbor(relatedPath[i + 1]);
                        relatedPathIndexes.Add(j);
                    }
                }

                rootPath.RemoveAt(i);
                List<List<NavGraphNode>> removedNeighbors = new List<List<NavGraphNode>>();
                for (int j = 0; j < i; j++)
                {
                    removedNeighbors.Add(rootPath[j].Neighbors);
                    foreach (NavGraphNode neighbor in rootPath[j].Neighbors)
                    {
                        neighbor.Neighbors.Remove(rootPath[j]);
                    }
                    rootPath[j].Neighbors = new List<NavGraphNode>();
                }

                NavPath spurPath = AStarShortestPath(spur, end);

                if (spurPath != null)
                {
                    rootPath.AddRange(spurPath.Nodes);

                    float totalPathLength = GetPathSegmentLength(rootPath, 0, i + 1) + spurPath.Length;
                    set.Add(new NavPath(rootPath, totalPathLength));
                }

                foreach (int pathIndex in relatedPathIndexes)
                {
                    kShortestPaths[pathIndex].Nodes[i].AddUndirectNeighbor(kShortestPaths[pathIndex].Nodes[i + 1]);
                }

                for (int j = 0; j < i; j++)
                {
                    rootPath[j].Neighbors = removedNeighbors[j];
                    foreach (NavGraphNode neighbor in rootPath[j].Neighbors)
                    {
                        neighbor.Neighbors.Add(rootPath[j]);
                    }
                }

            }

            if (set.Count == 0)
            {
                break;
            }


            var min = set.Min;
            set.Remove(min);
            kShortestPaths.Add(min);
        }

        return kShortestPaths;
    }

    private void IncreaseSearchId()
    {
        if (searchId == ulong.MaxValue)
        {
            ResetPathFindingData();
            searchId++;
        }
        searchId++;
    }

    private void ResetPathFindingData()
    {
        foreach (NavGraphNode node in Nodes)
        {
            node.SearchId = 0;
            node.HeapNode = null;
            node.Closed = false;
            node.From = null;
        }
    }

    public NavPath GetPrecalculatedPath(NavGraphNode start, NavGraphNode end)
    {
        if (!precalculationEnabled) 
            throw new InvalidOperationException("Precalculation must be enabled to reconstruct the path.");

        if (shortestPathTable[start.Index, end.Index].nodeIndex == -1) return null;

        List<NavGraphNode> path = new List<NavGraphNode>();
        int current = start.Index;

        while (current != end.Index)
        {
            path.Add(Nodes[current]);
            current = shortestPathTable[current, end.Index].nodeIndex;
        }
        path.Add(Nodes[current]);

        return new NavPath(path, shortestPathTable[start.Index, end.Index].length);
    }

    public float GetGraphDistance(NavGraphNode start, NavGraphNode end)
    {
        if (precalculationEnabled) return shortestPathTable[start.Index, end.Index].length;
        return AStarShortestPath(start, end).Length;
    }

    private static NavPath ReconstructPath(NavGraphNode end)
    {
        List<NavGraphNode> path = new List<NavGraphNode>();

        NavGraphNode current = end;
        while (current != null)
        {
            path.Add(current);
            current = current.From;
        }

        path.Reverse();
        return new NavPath(path, end.GCost);
    }

    public static float GetPathSegmentLength(List<NavGraphNode> path, int start, int count)
    {
        float length = 0;
        for (int i = start; i < count - 1; i++)
        {
            length += Vector3.Distance(path[i].transform.position, path[i + 1].transform.position);
        }
        return length;
    }

    public void PrecalculateShortestPaths()
    {
        shortestPathTable = new (int, float)[Nodes.Count, Nodes.Count];



        for (int source = 0; source < Nodes.Count; source++)
        {
            DijkstraShortestPath(Nodes[source], (current) => false);

            for (int target = 0; target < Nodes.Count; target++)
            {
                if (source == target) shortestPathTable[source, target] = (target, 0);
                else
                {
                    int nd = target;
                    while (nd != source && Nodes[nd].From != null && Nodes[nd].From.SearchId == searchId)
                    {
                        float length = Nodes[nd].GCost + Vector2.Distance(Nodes[nd].transform.position, Nodes[target].transform.position);
                        shortestPathTable[Nodes[nd].From.Index, target] = (nd, length);
                        nd = Nodes[nd].From.Index;
                    }
                }
            }
        }
    }

    public IEnumerable<NavGraphNode> TraverseDepthSearch(NavGraphNode start)
    {
        IncreaseSearchId();
        Stack<NavGraphNode> stack = new Stack<NavGraphNode>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            NavGraphNode current = stack.Pop();
            current.SearchId = searchId;

            yield return current;

            foreach (NavGraphNode node in current.Neighbors)
            {
                if (node.SearchId != searchId) stack.Push(node);
            }
        }
    }

    public NavGraphNode QuantizePosition(Vector2 origin, NavGraphNode coherenceNode = null)
    {
        if (coherenceNode)
        {
            coherenceNode.Neighbors.Add(coherenceNode);
            NavGraphNode quantizedPosition = QuantizePositionFromList(origin, coherenceNode.Neighbors);
            coherenceNode.Neighbors.RemoveAt(coherenceNode.Neighbors.Count - 1);
            return quantizedPosition;
        }
        return QuantizePositionFromList(origin, Nodes);
    }

    private NavGraphNode QuantizePositionFromList(Vector2 origin, IEnumerable<NavGraphNode> nodesToCheck)
    {
        NavGraphNode result = null;
        float minDistance = float.PositiveInfinity;

        foreach (NavGraphNode node in nodesToCheck)
        {
            Vector2 nodeVector = (Vector2)node.transform.position - origin;

            float distance = Vector2.Distance(origin, node.transform.position);
            if (distance < minDistance && !Physics2D.Raycast(origin, nodeVector, nodeVector.magnitude, WallMask))
            {
                minDistance = distance;
                result = node;
            }
        }

        return result;
    }


    private void OnDrawGizmosSelected()
    {
        if (!ShowEdges) return;
        if (Nodes == null) return;
        foreach (NavGraphNode node in Nodes)
        {
            Gizmos.color = Color.cyan;
            if (node.Neighbors == null) continue;
            foreach (NavGraphNode neighbor in node.Neighbors)
            {
                Gizmos.DrawLine(node.transform.position, neighbor.transform.position);
            }
        }

        if (TestPath != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < TestPath.Nodes.Count - 1; i++)
            {
                Gizmos.DrawLine(TestPath.Nodes[i].transform.position, TestPath.Nodes[i + 1].transform.position);
            }
        }
    }
}