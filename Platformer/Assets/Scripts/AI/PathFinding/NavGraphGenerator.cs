using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class NavGraphGenerator : MonoBehaviour
{
    [SerializeField]
    private LayerMask solidGeometryLayerMask;
    [SerializeField]
    private float expansionMargin = 0.001f;
    [SerializeField]
    private Sprite nodeSprite;
    [SerializeField]
    private NavGraph navGraph;
    [SerializeField]
    private float redundantPathMargin;

    public void GenerateNodes()
    {
        var mapColliders = GetComponentsInChildren<CompositeCollider2D>().Where(c => Utility.CheckLayer(c.gameObject.layer, solidGeometryLayerMask)).ToList();

        DeleteNodes();

        int graphLayer = LayerMask.NameToLayer("NavGraph");

        foreach (CompositeCollider2D collider in mapColliders)
        {
            string layerName = LayerMask.LayerToName(collider.gameObject.layer);

            for (int i = 0; i < collider.pathCount; i++)
            {
                Vector2[] pathPoints = new Vector2[collider.GetPathPointCount(i)];
                collider.GetPath(i, pathPoints);

                for (int j = 0; j < pathPoints.Length; j++)
                {
                    Vector2 edgeA = pathPoints[MathUtility.GetCircularIndex(j - 1, pathPoints.Length)] - pathPoints[j];
                    Vector2 edgeB = pathPoints[MathUtility.GetCircularIndex(j + 1, pathPoints.Length)] - pathPoints[j];

                    if (MathUtility.IsAngleConvexCC(edgeA, edgeB)) continue;

                    Vector2 normalA = edgeA.Perpendicular2().normalized;
                    Vector2 normalB = edgeB.Perpendicular1().normalized;

                    Vector2 expansionVector = (normalA + normalB).normalized;
                    Vector2 nodePosition = pathPoints[j] + expansionVector * expansionMargin;

                    if (!mapColliders.Any(c => MathUtility.IsPointInsideCompositeCollider(nodePosition, c)))
                    {
                        GameObject nodeObject = new GameObject();
                        nodeObject.transform.parent = navGraph.transform;
                        nodeObject.layer = graphLayer;

                        SpriteRenderer renderer = nodeObject.AddComponent<SpriteRenderer>();
                        renderer.sprite = nodeSprite;
                        renderer.sortingLayerName = "UI";

                        NavGraphNode node = nodeObject.AddComponent<NavGraphNode>();
                        node.Index = navGraph.Nodes.Count;
                        node.ExpansionVector = expansionVector;
                        node.transform.position = nodePosition;
                        node.name = $"{layerName} {i}-{j}";

                        navGraph.Nodes.Add(node);
                    }
                }
            }
        }

        Debug.Log($"Node Count: {navGraph.Nodes.Count}");
    }

    public void DeleteNodes()
    {
        foreach (NavGraphNode node in navGraph.Nodes)
        {
            DestroyImmediate(node.gameObject);
        }
        navGraph.Nodes.Clear();
        navGraph.TestPath.Clear();
    }

    public void GenerateEdges()
    {
        int edgeCount = 0;

        foreach (NavGraphNode node in navGraph.Nodes)
        {
            foreach (NavGraphNode neighbor in navGraph.Nodes)
            {
                if (node == neighbor) continue;

                Vector2 direction = neighbor.transform.position - node.transform.position;
                RaycastHit2D hit = Physics2D.Raycast(node.transform.position, direction.normalized, direction.magnitude, solidGeometryLayerMask);

                if (hit.collider == null)
                {
                    node.Neighbors.Add(neighbor);
                    edgeCount++;
                }
            }
        }

        Debug.Log($"Edge Count: {edgeCount / 2}");
    }

    public void DeleteEdges()
    {
        foreach (NavGraphNode node in navGraph.Nodes)
        {
            node.Neighbors.Clear();
        }
    }

    public void DeleteRedundantNodes()
    {
        List<NavGraphNode>[,] kShortestPathTable = CalculateKShortestPathTable();

        HashSet<NavGraphNode> redundantNodes = new HashSet<NavGraphNode>();

        foreach (NavGraphNode node in navGraph.Nodes)
        {
            redundantNodes.Add(node);
        }

        for (int i = 0; i < navGraph.Nodes.Count; i++)
        {
            for (int j = 0; j < navGraph.Nodes.Count; j++)
            {
                List<NavGraphNode> kShortestPaths = kShortestPathTable[i, j];

                if (i != j && kShortestPaths.Count != 0)
                {
                    if (kShortestPaths.Count == 1)
                    {
                        redundantNodes.Remove(kShortestPaths[0]);
                    }
                }
            }
        }

        foreach (NavGraphNode node in redundantNodes)
        {
            foreach (NavGraphNode neighbor in node.Neighbors)
            {
                neighbor.Neighbors.Remove(node);
            }
            node.Neighbors.Clear();
            navGraph.Nodes.Remove(node);
            DestroyImmediate(node.gameObject);
        }
    }

    private List<NavGraphNode>[,] CalculateKShortestPathTable()
    {
        List<NavGraphNode>[,] kShortestPathTable = new List<NavGraphNode>[navGraph.Nodes.Count, navGraph.Nodes.Count];

        for (int source = 0; source < navGraph.Nodes.Count; source++)
        {
            for (int target = 0; target < navGraph.Nodes.Count; target++)
            {
                if (target == source) continue;

                List<(List<NavGraphNode> path, float length)> shortestPaths = navGraph.YenKShortestPaths
                (
                    navGraph.Nodes[source],
                    navGraph.Nodes[target],
                    (k, paths) => paths[k].length - paths[k - 1].length < redundantPathMargin
                );

                foreach ((List<NavGraphNode> path, float length) in shortestPaths)
                {
                    kShortestPathTable[source, target].Add(path[path.Count - 2]);
                }
            }
        }

        return kShortestPathTable;
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

        if (GUILayout.Button("Delete Redundant Nodes")) script.DeleteRedundantNodes();
    }
}

#endif
