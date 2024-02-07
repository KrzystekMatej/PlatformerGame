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
    private float expansionMargin = 0.001f;
    [SerializeField]
    private Sprite nodeSprite;
    [SerializeField]
    private NavGraph navGraph;
    [SerializeField]
    private float minNodeDistance = Mathf.Epsilon;
    [SerializeField]
    private float redundantPathMargin = Mathf.Epsilon;
    [SerializeField]
    [HideInInspector]
    private GameObject nodeContainer;

    public void GenerateNodes()
    {
        CompositeCollider2D[] solidColliders = GetComponentsInChildren<CompositeCollider2D>().Where(c => Utility.CheckLayer(c.gameObject.layer, navGraph.SolidGeometryLayerMask)).ToArray();
        Collider2D[] traversableZones = navGraph.GetComponentsInChildren<Collider2D>();

        DeleteNodes();
       
        nodeContainer = new GameObject("Nodes");
        nodeContainer.transform.parent = navGraph.transform;

        int graphLayer = LayerMask.NameToLayer("NavGraph");

        foreach (CompositeCollider2D collider in solidColliders)
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

                    if (traversableZones.Any(z => z.OverlapPoint(nodePosition)) && !solidColliders.Any(c => MathUtility.IsPointInsideCompositeCollider(nodePosition, c)))
                    {
                        GameObject nodeObject = new GameObject();
                        nodeObject.transform.parent = nodeContainer.transform;
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

        DeleteCloseNodes();
        Debug.Log($"Number of generated nodes is {navGraph.Nodes.Count}.");
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
                RaycastHit2D hit = Physics2D.Raycast(node.transform.position, direction.normalized, direction.magnitude, navGraph.SolidGeometryLayerMask);

                if (hit.collider == null)
                {
                    node.Neighbors.Add(neighbor);
                    edgeCount++;
                }
            }
        }

        Debug.Log($"Number of generated edges is {edgeCount / 2}.");
    }

    public void DeleteNodes()
    {
        navGraph.Nodes.Clear();
        navGraph.TestPath = null;
        DestroyImmediate(nodeContainer);
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

                if (i != j && !navGraph.Nodes[i].Neighbors.Any(n => n.Index == j) && kShortestPaths != null)
                {
                    if (kShortestPaths.Count == 1)
                    {
                        redundantNodes.Remove(kShortestPaths[0]);
                    }
                }
            }
        }

        DeleteNodesInEnumerable(redundantNodes);
    }

    private void DeleteCloseNodes()
    {
        List<NavGraphNode> redundantNodes = new List<NavGraphNode>();

        for (int i = 0; i < navGraph.Nodes.Count; i++)
        {
            for (int j = i + 1; j < navGraph.Nodes.Count; j++)
            {
                if (Vector3.Distance(navGraph.Nodes[i].transform.position, navGraph.Nodes[j].transform.position) <= minNodeDistance)
                {
                    redundantNodes.Add(navGraph.Nodes[i]);
                    redundantNodes.Add(navGraph.Nodes[j]);
                }
            }
        }

        DeleteNodesInEnumerable(redundantNodes);
        Debug.Log($"Number of deleted nodes which were too close to each other is {redundantNodes.Count}.");
    }

    private void AssignIndexes()
    {
        for (int i = 0; i < navGraph.Nodes.Count; i++)
        {
            navGraph.Nodes[i].Index = i;
        }
    }

    private void DeleteNodesInEnumerable(IEnumerable nodes)
    {
        foreach (NavGraphNode node in nodes)
        {
            foreach (NavGraphNode neighbor in node.Neighbors)
            {
                neighbor.Neighbors.Remove(node);
            }
            node.Neighbors.Clear();
            navGraph.Nodes.Remove(node);
            DestroyImmediate(node.gameObject);

            if (navGraph.TestPath != null && navGraph.TestPath.Nodes.Contains(node)) navGraph.TestPath = null;
        }

        AssignIndexes();
    }

    private List<NavGraphNode>[,] CalculateKShortestPathTable()
    {
        List<NavGraphNode>[,] kShortestPathTable = new List<NavGraphNode>[navGraph.Nodes.Count, navGraph.Nodes.Count];

        for (int source = 0; source < navGraph.Nodes.Count; source++)
        {
            for (int target = 0; target < navGraph.Nodes.Count; target++)
            {
                if (target == source)
                {
                    kShortestPathTable[source, target] = new List<NavGraphNode>() { navGraph.Nodes[source] };
                }

                List<NavPath> shortestPaths = navGraph.YenShortestPaths
                (
                    navGraph.Nodes[source],
                    navGraph.Nodes[target],
                    (k, paths) => (k == 1) ? true : paths[k - 1].Length - paths[k - 2].Length <= redundantPathMargin
                );

                if (shortestPaths != null)
                {
                    kShortestPathTable[source, target] = shortestPaths.Select(path => path.Nodes[1]).ToList();
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
