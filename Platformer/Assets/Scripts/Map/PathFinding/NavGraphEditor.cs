using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(NavGraph))]
public class NavGraphEditor : Editor
{
    private NavGraphNode start;
    private NavGraphNode end;
    private int k;

    public override void OnInspectorGUI()
    {
        NavGraph navGraph = (NavGraph)target;

        serializedObject.Update();

        DrawDefaultInspector();

        start = (NavGraphNode)EditorGUILayout.ObjectField("Start", start, typeof(NavGraphNode), true);
        end = (NavGraphNode)EditorGUILayout.ObjectField("End", end, typeof(NavGraphNode), true);

        EditorGUILayout.Space();

        if (GUILayout.Button("Calculate Shortest Path"))
        {
            if (start == null || end == null)
            {
                Debug.Log("No node can be null.");
                return;
            }
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var path = navGraph.AStarShortestPath(start, end);
            if (path.Nodes != null)
            {
                navGraph.TestPath = path;
            }
            else
            {
                navGraph.TestPath = null;
                Debug.Log("Path does not exist.");
            }
            stopwatch.Stop();
            Debug.Log($"Method execution time is {stopwatch.Elapsed.TotalSeconds} seconds.");
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        k = EditorGUILayout.IntField("K", k);

        EditorGUILayout.Space();

        if (GUILayout.Button("Calculate Kth Shortest Path"))
        {
            if (start == null || end == null)
            {
                Debug.Log("No node can be null.");
                return;
            }
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var paths = navGraph.YenKShortestPaths(start, end, k);
            stopwatch.Stop();
            if (paths != null && paths.Count > k)
            {
                navGraph.TestPath = paths[k];
            }
            else
            {
                navGraph.TestPath = null;
                Debug.Log("Path does not exist.");
            }
            Debug.Log($"Method execution time is {stopwatch.Elapsed.TotalSeconds} seconds.");
        }

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Nodes")) GenerateNodes(navGraph);
        if (GUILayout.Button("Delete Nodes")) DeleteNodes(navGraph);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Edges")) GenerateEdges(navGraph);
        if (GUILayout.Button("Delete Edges")) DeleteEdges(navGraph);
        GUILayout.EndHorizontal();

        Undo.RecordObject(navGraph, "Modify Object");
    }

    public void GenerateNodes(NavGraph navGraph)
    {
        SerializedProperty nodesProperty = serializedObject.FindProperty("Nodes");

        DeleteNodes(navGraph);

        GameObject nodeContainer = new GameObject("Nodes");
        nodeContainer.transform.parent = navGraph.transform;
        serializedObject.FindProperty("NodeContainer").SetValue(nodeContainer);
        serializedObject.ApplyModifiedProperties();


        List<List<Vector2[]>> colliderPathCollection = navGraph.Walls.Select(c => MathUtility.GetColliderPaths(c, navGraph.CirclePathRatio)).ToList();


        
        for (int i = 0; i < navGraph.Walls.Count; i++)
        {
            List<Vector2[]> paths = colliderPathCollection[i];

            for (int j = 0; j < paths.Count; j++)
            {
                Vector2[] points = paths[j];
                int pathNodeCount = 0;

                for (int k = 0; k < points.Length; k++)
                {
                    Vector2 ingoing = (points[k] - points[MathUtility.GetCircularIndex(k - 1, points.Length)]).normalized;
                    Vector2 outgoing = (points[MathUtility.GetCircularIndex(k + 1, points.Length)] - points[k]).normalized;

                    if (MathUtility.IsAngleConvexCC(-ingoing, outgoing)) continue;
                    if (ingoing == Vector2.zero || outgoing == Vector2.zero) Debug.Log("ahoj");

                    Vector2 nodePosition = points[k] + MathUtility.GetExpansionOffsetCC(ingoing, outgoing, navGraph.CollisionAvoidanceMargin);

                    bool isInZone = navGraph.TraversableZones.Count == 0 || navGraph.TraversableZones.Any(z => z.OverlapPoint(nodePosition));
                    bool isInCollider = colliderPathCollection
                        .Any(c => c.Any(p => MathUtility.IsPointInsidePolygon(nodePosition, p)));


                    if (isInZone && !isInCollider)
                    {
                        GameObject nodeObject = new GameObject();
                        nodeObject.transform.parent = nodeContainer.transform;

                        SpriteRenderer renderer = nodeObject.AddComponent<SpriteRenderer>();
                        renderer.sprite = navGraph.NodeSprite;
                        renderer.sortingLayerName = "UI";

                        NavGraphNode node = nodeObject.AddComponent<NavGraphNode>();
                        node.transform.position = nodePosition;
                        node.name = $"{navGraph.Walls[i].gameObject.name} {j}-{pathNodeCount}";

                        SerializedObject serializedNode = new SerializedObject(node);
                        serializedNode.FindProperty("Ingoing").SetValue(ingoing);
                        serializedNode.FindProperty("Outgoing").SetValue(outgoing);
                        serializedNode.FindProperty("Index").SetValue(nodesProperty.arraySize);
                        serializedNode.ApplyModifiedProperties();

                        nodesProperty.ArrayAdd(node);
                        serializedObject.ApplyModifiedProperties();

                        pathNodeCount++;
                    }
                }
            }
        }

        DeleteStackedNodes(navGraph, nodesProperty);
        Debug.Log($"Number of generated nodes is {nodesProperty.arraySize}.");
    }

    public void DeleteNodes(NavGraph navGraph)
    {
        SerializedProperty nodesProperty = serializedObject.FindProperty("Nodes");
        nodesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
        DestroyImmediate(serializedObject.FindProperty("NodeContainer").GetValue<GameObject>());
        navGraph.TestPath = new NavPath();
    }

    private void DeleteStackedNodes(NavGraph navGraph, SerializedProperty nodesProperty)
    {
        const float safetyDistance = 0.001f;
        HashSet<int> indexSet = new HashSet<int>();
        for (int i = 0; i < nodesProperty.arraySize; i++)
        {
            for (int j = i + 1; j < nodesProperty.arraySize; j++)
            {
                NavGraphNode nodeA = nodesProperty.ArrayGet<NavGraphNode>(i);
                NavGraphNode nodeB = nodesProperty.ArrayGet<NavGraphNode>(j);

                bool areClose = safetyDistance >= Vector3.Distance
                (
                    nodeA.GetExpandedPosition(-navGraph.CollisionAvoidanceMargin),
                    nodeB.GetExpandedPosition(-navGraph.CollisionAvoidanceMargin)
                );

                if (areClose)
                {
                    indexSet.Add(nodeA.Index);
                    indexSet.Add(nodeB.Index);
                }

            }
        }

        List<int> descendingIndexes = indexSet.ToList();
        descendingIndexes.Sort((a, b) => b.CompareTo(a));

        foreach (int destroyIndex in descendingIndexes)
        {
            DestroyImmediate(nodesProperty.ArrayGet<NavGraphNode>(destroyIndex).gameObject);
            nodesProperty.ArrayRemoveAt(destroyIndex);
        }

        serializedObject.ApplyModifiedProperties();
        ReassignIndexes(nodesProperty);


        Debug.Log($"Number of deleted nodes which were stacked on each other is {indexSet.Count}.");
    }

    private void ReassignIndexes(SerializedProperty nodesProperty)
    {
        for (int i = 0; i < nodesProperty.arraySize; i++)
        {
            SerializedObject serializedNode = new SerializedObject(nodesProperty.ArrayGet<NavGraphNode>(i));
            serializedNode.FindProperty("Index").SetValue(i);
            serializedNode.ApplyModifiedProperties();
        }
    }


    public void GenerateEdges(NavGraph navGraph)
    {
        DeleteEdges(navGraph);
        SerializedProperty nodesProperty = serializedObject.FindProperty("Nodes");
        int edgeCount = 0;

        for (int i = 0; i < nodesProperty.arraySize; i++)
        {
            for (int j = i + 1; j < nodesProperty.arraySize; j++)
            {
                NavGraphNode nodeA = nodesProperty.ArrayGet<NavGraphNode>(i);
                NavGraphNode nodeB = nodesProperty.ArrayGet<NavGraphNode>(j);
                Vector2 positionA = nodeA.GetExpandedPosition(navGraph.MaxAllowedRadius);
                Vector2 positionB = nodeB.GetExpandedPosition(navGraph.MaxAllowedRadius);
                Vector2 edgeVector = positionB - positionA;
                RaycastHit2D hit = Physics2D.CircleCast(positionA, navGraph.MaxAllowedRadius, edgeVector, edgeVector.magnitude, navGraph.WallMask);

                if (!hit)
                {
                    SerializedObject nodeAObject = new SerializedObject(nodeA);
                    SerializedObject nodeBObject = new SerializedObject(nodeB);
                    nodeAObject.FindProperty("Neighbors").ArrayAdd(nodeB);
                    nodeBObject.FindProperty("Neighbors").ArrayAdd(nodeA);
                    nodeAObject.ApplyModifiedProperties();
                    nodeBObject.ApplyModifiedProperties();
                    edgeCount++;
                }
            }
        }

        Debug.Log($"Number of generated edges is {edgeCount}.");
    }

    public void DeleteEdges(NavGraph navGraph)
    {
        SerializedProperty nodesProperty = serializedObject.FindProperty("Nodes");

        for (int i = 0; i < nodesProperty.arraySize; i++)
        {
            SerializedObject nodeAObject = new SerializedObject(nodesProperty.ArrayGet<NavGraphNode>(i));
            nodeAObject.FindProperty("Neighbors").ClearArray();
            nodeAObject.ApplyModifiedProperties();
        }
    }
}
