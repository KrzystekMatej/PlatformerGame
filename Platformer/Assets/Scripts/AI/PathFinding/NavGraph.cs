using FibonacciHeap;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NavGraph : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private bool showEdges;
    [HideInInspector]
    public List<NavGraphNode> TestPath;
#endif

    [HideInInspector]
    public List<NavGraphNode> Nodes = new List<NavGraphNode>();
    private ulong processId;
    [SerializeField]
    private bool precalculationEnabled;
    private NavGraphNode[,] shortestPathTable;

    private void Awake()
    {
        if (precalculationEnabled) PrecalculateShortestPaths();
    }


    public (List<NavGraphNode> path, float length) DijkstraShortestPath(NavGraphNode start, Func<NavGraphNode, bool> terminationCondition)
    {
        return FindShortestPath
        (
            start,
            terminationCondition,
            (current) => 0
        );
    }

    public (List<NavGraphNode> path, float length) AStarShortestPath(NavGraphNode start, NavGraphNode end)
    {
        return FindShortestPath
        (
            start,
            (current) => current == end,
            (current) => Vector2.Distance(current.transform.position, end.transform.position)
        );
    }

    public (List<NavGraphNode> path, float length) FindShortestPath(NavGraphNode start, Func<NavGraphNode, bool> terminationCondition, Func<NavGraphNode, float> heuristic)
    {
        if (processId == ulong.MaxValue)
        {
            ResetPathFindingData();
            processId++;
        }
        processId++;

        FibonacciHeap<NavGraphNode, float> heap = new FibonacciHeap<NavGraphNode, float>(float.NegativeInfinity);

        start.ProcessId = processId;
        start.GCost = 0;
        start.HCost = heuristic(start);
        start.HeapNode = new FibonacciHeapNode<NavGraphNode, float>(start, start.HCost);
        start.Closed = false;
        start.From = null;
        heap.Insert(start.HeapNode);

        while (!heap.IsEmpty())
        {
            NavGraphNode currentNode = heap.Min().Data;

            if (terminationCondition(currentNode)) return (ReconstructPath(currentNode), currentNode.GCost);

            foreach (NavGraphNode neighbor in currentNode.Neighbors)
            {
                float neighborGCost = currentNode.GCost + Vector2.Distance(currentNode.transform.position, neighbor.transform.position);

                if (neighbor.ProcessId == processId)
                {
                    if (neighbor.Closed) continue;

                    if (neighborGCost < neighbor.GCost) heap.DecreaseKey(neighbor.HeapNode, neighborGCost + neighbor.HCost);
                    else continue;
                }
                else
                {
                    neighbor.ProcessId = processId;
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

        return (null, 0);
    }

    public List<(List<NavGraphNode> path, float length)> YenKShortestPaths(NavGraphNode start, NavGraphNode end, int k) => YenKShortestPaths(start, end, (i, paths) => i <= k);

    public List<(List<NavGraphNode> path, float length)> YenKShortestPaths(NavGraphNode start, NavGraphNode end, Func<int, List<(List<NavGraphNode> path, float length)>, bool> terminationCondition)
    {
        List<(List<NavGraphNode> path, float length)> kShortestPaths = new List<(List<NavGraphNode>, float)>() { AStarShortestPath(start, end) };
        FibonacciHeap<List<NavGraphNode>, float> heap = new FibonacciHeap<List<NavGraphNode>, float>(float.NegativeInfinity);

        
        for (int k = 1; terminationCondition(k, kShortestPaths); k++)
        {
            for (int i = 0; i <= kShortestPaths[k - 1].path.Count - 2; i++)
            {
                NavGraphNode spur = kShortestPaths[k - 1].path[i];
                List<NavGraphNode> rootPath = kShortestPaths[k - 1].path.GetRange(0, i + 1);

                List<int> relatedPathIndexes = new List<int>();
                for (int j = 0; j < kShortestPaths.Count; j++)
                {
                    List<NavGraphNode> relatedPath = kShortestPaths[j].path;
                    if (relatedPath.Count < i + 1) continue;
                    if (rootPath.SequenceEqual(relatedPath.GetRange(0, i + 1)))
                    {
                        relatedPath[i].RemoveNeighbor(relatedPath[i + 1]);
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

                (List<NavGraphNode> spurPath, float spurPathLength) = AStarShortestPath(spur, end);

                if (spurPath != null)
                {
                    rootPath.AddRange(spurPath);

                    float totalPathLength = GetPathSegmentLength(rootPath, 0, i + 1) + spurPathLength;

                    bool contains = false;
                    //does it iterate through?
                    foreach (FibonacciHeapNode<List<NavGraphNode>, float> heapNode in heap)
                    {
                        if (totalPathLength != heapNode.Key) continue;
                        if (heapNode.Data.SequenceEqual(rootPath))
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (!contains) heap.Insert(new FibonacciHeapNode<List<NavGraphNode>, float>(rootPath, totalPathLength));

                }

                foreach (int pathIndex in relatedPathIndexes)
                {
                    kShortestPaths[pathIndex].path[i].AddNeighbor(kShortestPaths[pathIndex].path[i + 1]);
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

            if (heap.IsEmpty())
            {
                break;
            }


            var min = heap.RemoveMin();
            kShortestPaths.Add((min.Data, min.Key));
        }

        return kShortestPaths;
    }

    public void ResetPathFindingData()
    {
        foreach (NavGraphNode node in Nodes)
        {
            node.ProcessId = 0;
            node.HeapNode = null;
            node.Closed = false;
            node.From = null;
        }
    }

    public List<NavGraphNode> ReconstructPrecalculatedPath(NavGraphNode start, NavGraphNode end)
    {
        if (!precalculationEnabled) 
            throw new InvalidOperationException("Precalculation must be enabled to reconstruct the path.");

        if (shortestPathTable[start.Index, end.Index] == null) return null;

        List<NavGraphNode> path = new List<NavGraphNode>();
        NavGraphNode current = start;

        while (current != end)
        {
            path.Add(current);
            current = shortestPathTable[current.Index, end.Index];
        }
        path.Add(current);

        return path;
    }

    private static List<NavGraphNode> ReconstructPath(NavGraphNode current)
    {
        List<NavGraphNode> path = new List<NavGraphNode>();

        while (current != null)
        {
            path.Add(current);
            current = current.From;
        }

        path.Reverse();
        return path;
    }

    public static float GetPathSegmentLength(List<NavGraphNode> path, int start, int count)
    {
        float length = 0;
        for (int i = start; i < count - 1; i++)
        {
            length += Vector3.Distance(path[i].transform.position, path[i + 1].transform.position);
        }
        int a = 0;
        return length;
    }

    public void PrecalculateShortestPaths()
    {
        shortestPathTable = new NavGraphNode[Nodes.Count, Nodes.Count];

        for (int source = 0; source < Nodes.Count; source++)
        {
            DijkstraShortestPath(Nodes[source], (current) => false);

            for (int target = 0; target < Nodes.Count; target++)
            {
                if (source == target) shortestPathTable[source, target] = Nodes[target];
                else
                {
                    int nd = target;
                    while (nd != source && Nodes[nd].From != null && Nodes[nd].From.ProcessId == processId)
                    {
                        shortestPathTable[Nodes[nd].From.Index, target] = Nodes[nd];
                        nd = Nodes[nd].From.Index;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Nodes = GetComponentsInChildren<NavGraphNode>().ToList();

        if (!showEdges) return;
        foreach (NavGraphNode node in Nodes)
        {
            Gizmos.color = Color.cyan;
            foreach (NavGraphNode neighbor in node.Neighbors)
            {
                Gizmos.DrawLine(node.transform.position, neighbor.transform.position);
            }
        }

        if (TestPath != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < TestPath.Count - 1; i++)
            {
                Gizmos.DrawLine(TestPath[i].transform.position, TestPath[i + 1].transform.position);
            }
        }
    }
}


[CustomEditor(typeof(NavGraph))]
public class NavGraphEditor : Editor
{
    private NavGraphNode start;
    private NavGraphNode end;
    private int k;
    private SerializedProperty testPathProperty;
    private float testPathLength;

    void OnEnable()
    {
        testPathProperty = serializedObject.FindProperty("TestPath");
    }

    public override void OnInspectorGUI()
    {
        NavGraph script = (NavGraph)target;

        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUIStyle centeredStyle = new GUIStyle(EditorStyles.label);
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("Tests", centeredStyle);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        start = (NavGraphNode)EditorGUILayout.ObjectField("Start", start, typeof(NavGraphNode), true);
        end = (NavGraphNode)EditorGUILayout.ObjectField("End", end, typeof(NavGraphNode), true);

        EditorGUILayout.Space();

        if (GUILayout.Button("Calculate shortest path"))
        {
            if (start == null || end == null)
            {
                Debug.Log("No node can be null.");
                return;
            }
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            (script.TestPath, testPathLength) = script.AStarShortestPath(start, end);
            stopwatch.Stop();
            Debug.Log($"Method execution time: {stopwatch.Elapsed.TotalSeconds} seconds");
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        k = EditorGUILayout.IntField("K", k);

        EditorGUILayout.Space();

        if (GUILayout.Button("Calculate Kth shortest path"))
        {
            if (start == null || end == null)
            {
                Debug.Log("No node can be null.");
                return;
            }
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var paths = script.YenKShortestPaths(start, end, k);
            (script.TestPath, testPathLength) = paths[k];
            stopwatch.Stop();
            Debug.Log($"Method execution time: {stopwatch.Elapsed.TotalSeconds} seconds");

            for (int i = 0; i < paths.Count; i++)
            {
                for (int j = i + 1; j < paths.Count; j++)
                {
                    if (paths[i].path.SequenceEqual(paths[j].path))
                    {
                        Debug.Log($"duplicate {i} {j}");
                    }
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(testPathProperty, new GUIContent("Test Path"), true);
        EditorGUILayout.FloatField("Test Path Length", NavGraph.GetPathSegmentLength(script.TestPath, 0, script.TestPath.Count));

        serializedObject.ApplyModifiedProperties();
    }
}