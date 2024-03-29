using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
public class NavGraphGenerator : MonoBehaviour
{
    [SerializeField]
    private NavGraph navGraph;
    [SerializeField]
    private float collisionAvoidanceMargin = 0.1f;
    [SerializeField]
    private float maxAllowedRadius = 1f;
    [SerializeField]
    private Sprite nodeSprite;
    [SerializeField]
    private float minNodeDistance = Mathf.Epsilon;
    [SerializeField]
    private float circlePathRatio = 0.5f;


    [SerializeField]
    private GameObject solidGeometryContainer;
    [SerializeField]
    [HideInInspector]
    private GameObject nodeContainer;

    public void GenerateNodes()
    {
        SerializedObject navGraphObject = new SerializedObject(navGraph);
        SerializedProperty nodesProperty = navGraphObject.FindProperty("Nodes");

        Collider2D[] solidColliders = solidGeometryContainer
            .GetComponentsInChildren<Collider2D>()
            .Where(c => Utility.CheckLayer(c.gameObject.layer, navGraph.WallMask) && c is not TilemapCollider2D)
            .ToArray();

        Collider2D[] traversableZones = navGraph.GetComponentsInChildren<Collider2D>();

        DeleteNodes();
       
        nodeContainer = new GameObject("Nodes");
        nodeContainer.transform.parent = navGraph.transform;


        List<List<Vector2[]>> colliderPathCollection = solidColliders.Select(c => MathUtility.GetColliderPaths(c, circlePathRatio)).ToList();



        for (int i = 0; i < solidColliders.Length; i++)
        {
            List<Vector2[]> paths = colliderPathCollection[i];

            for (int j = 0; j < paths.Count; j++)
            {
                Vector2[] points = paths[j];
                int nodeCount = 0;

                for (int k = 0; k < points.Length; k++)
                {
                    Vector2 ingoing = (points[k] - points[MathUtility.GetCircularIndex(k - 1, points.Length)]).normalized;
                    Vector2 outgoing = (points[MathUtility.GetCircularIndex(k + 1, points.Length)] - points[k]).normalized;

                    if (MathUtility.IsAngleConvexCC(-ingoing, outgoing)) continue;
                    if (ingoing == Vector2.zero || outgoing == Vector2.zero) Debug.Log("ahoj");

                    Vector2 nodePosition = points[k] + MathUtility.GetExpansionOffsetCC(ingoing, outgoing, collisionAvoidanceMargin);

                    bool isInZone = traversableZones.Any(z => z.OverlapPoint(nodePosition));
                    bool isInCollider = colliderPathCollection
                        .Any(c => c.Any(p => MathUtility.IsPointInsidePolygon(nodePosition, p)));

                    
                    if (isInZone && !isInCollider)
                    {
                        GameObject nodeObject = new GameObject();
                        nodeObject.transform.parent = nodeContainer.transform;

                        SpriteRenderer renderer = nodeObject.AddComponent<SpriteRenderer>();
                        renderer.sprite = nodeSprite;
                        renderer.sortingLayerName = "UI";

                        NavGraphNode node = nodeObject.AddComponent<NavGraphNode>();
                        node.transform.position = nodePosition;
                        node.name = $"{solidColliders[i].gameObject.name} {j}-{nodeCount}";

                        SerializedObject serializedNode = new SerializedObject(node);
                        serializedNode.FindProperty("Ingoing").SetValue(ingoing);
                        serializedNode.FindProperty("Outgoing").SetValue(outgoing);
                        serializedNode.FindProperty("Index").SetValue(nodeCount);

                        nodesProperty.ArrayAdd(node);
                        navGraphObject.ApplyModifiedProperties();

                        nodeCount++;
                    }
                }
            }
        }

        //DeleteCloseNodes(navGraphObject, nodesProperty);
        Debug.Log($"Number of generated nodes is {navGraph.Nodes.Count}.");
    }

    public void GenerateEdges()
    {
        int edgeCount = 0;

        for (int i = 0; i < navGraph.Nodes.Count; i++)
        {
            for (int j = i + 1; j < navGraph.Nodes.Count; j++)
            {
                Vector2 expandedNode = navGraph.Nodes[i].GetExpandedPosition(maxAllowedRadius);
                Vector2 expandedNeighbor = navGraph.Nodes[j].GetExpandedPosition(maxAllowedRadius);
                Vector2 neighborVector = expandedNeighbor - expandedNode;
                RaycastHit2D hit = Physics2D.CircleCast(expandedNode, maxAllowedRadius, neighborVector, neighborVector.magnitude, navGraph.WallMask);

                if (!hit)
                {
                    navGraph.Nodes[i].Neighbors.Add(navGraph.Nodes[j]);
                    navGraph.Nodes[j].Neighbors.Add(navGraph.Nodes[i]);
                    edgeCount++;
                }
            }
        }

        Debug.Log($"Number of generated edges is {edgeCount}.");
    }

    public void DeleteNodes()
    {
        SerializedObject serializedObject = new SerializedObject(navGraph);
        SerializedProperty nodesProperty = serializedObject.FindProperty("Nodes");
        nodesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
        navGraph.TestPath = new NavPath();
        DestroyImmediate(nodeContainer);
    }


    public void DeleteEdges()
    {
        foreach (NavGraphNode node in navGraph.Nodes)
        {
            node.Neighbors.Clear();
        }
    }

    private void DeleteCloseNodes(SerializedObject navGraphObject, SerializedProperty nodesProperty)
    {
        List<NavGraphNode> nodes = nodesProperty.ArrayGetElements().Select(p => p.GetValue<NavGraphNode>()).ToList();

        int i = 0;
        int count = 0;
        int deletedCount = 0;
        while (i < nodesProperty.arraySize)
        {
            NavGraphNode nodeA = nodesProperty.ArrayGet<NavGraphNode>(i);

            if (nodesProperty.ArrayRemove<NavGraphNode>(n => AreClose(nodeA, n)))
            {
                i = 0;
                navGraphObject.ApplyModifiedProperties();
                ReassignIndexes(nodesProperty);
                deletedCount++;
            }
            else break;

            if (count > 1000)
            {
                Debug.Log("Bad");
                break;
            }

            i++;
            count++;
        }

        Debug.Log($"Number of deleted nodes which were too close to each other is {deletedCount}.");
    }

    private bool AreClose(NavGraphNode nodeA, NavGraphNode nodeB)
    {
        bool isDifferent = nodeB.Index != nodeA.Index;
        bool isClose = Vector3.Distance(nodeA.GetExpandedPosition(-collisionAvoidanceMargin), nodeB.GetExpandedPosition(-collisionAvoidanceMargin)) <= minNodeDistance;
        return isDifferent && isClose;
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
}


[CustomEditor(typeof(NavGraphGenerator))]
public class NavGraphGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NavGraphGenerator script = (NavGraphGenerator)target;

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Nodes")) script.GenerateNodes();
        if (GUILayout.Button("Delete Nodes")) script.DeleteNodes();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Edges")) script.GenerateEdges();
        if (GUILayout.Button("Delete Edges")) script.DeleteEdges();
        GUILayout.EndHorizontal();

        Undo.RecordObject(script, "Modify Object");
    }
}

#endif
