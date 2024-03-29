using FibonacciHeap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using HeapNode = FibonacciHeap.FibonacciHeapNode<NavGraphNode, float>;

public class NavGraphNode : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Ingoing;
    [HideInInspector]
    public Vector2 Outgoing;
    [HideInInspector]
    public int Index;
    [HideInInspector]
    public List<NavGraphNode> Neighbors = new List<NavGraphNode>();

    [NonSerialized]
    public ulong SearchId;
    [NonSerialized]
    public HeapNode HeapNode;
    [NonSerialized]
    public float GCost;
    [NonSerialized]
    public float HCost;
    [NonSerialized]
    public bool Closed;
    [NonSerialized]
    public NavGraphNode From;

private void Start()
    {
#if !UNITY_EDITOR
         Destroy(GetComponent<SpriteRenderer>());
#endif
    }

    public void AddDirectNeighbor(NavGraphNode neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }


    public void RemoveDirectNeighbor(NavGraphNode neighbor)
    {
        Neighbors.Remove(neighbor);
    }

    public void AddUndirectNeighbor(NavGraphNode neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
            if (!neighbor.Neighbors.Contains(this))
            {
                neighbor.Neighbors.Add(this);
            }
        }
    }


    public void RemoveUndirectNeighbor(NavGraphNode neighbor)
    {
        Neighbors.Remove(neighbor);
        neighbor.Neighbors.Remove(this);
    }

    public Vector2 GetExpandedPosition(float radius)
    {
        return transform.position + (Vector3)MathUtility.GetExpansionOffsetCC(Ingoing, Outgoing, radius);
    }
}

[CustomEditor(typeof(NavGraphNode))]
public class NavGraphNodeEditor : Editor
{
    private NavGraphNode newNeighbor;

    public override void OnInspectorGUI()
    {
        NavGraphNode node = (NavGraphNode)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        newNeighbor = (NavGraphNode)EditorGUILayout.ObjectField("Add New Neighbor", newNeighbor, typeof(NavGraphNode), true);

        if (newNeighbor && GUILayout.Button("Add Edge"))
        {
            Undo.RecordObject(node, "Add Edge");

            node.AddUndirectNeighbor(newNeighbor);
            Undo.RecordObject(newNeighbor, "Add Edge");
            newNeighbor.AddUndirectNeighbor(node);

            newNeighbor = null;
        }

        if (node.Neighbors.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Neighbors:");
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(node.Neighbors[i], typeof(NavGraphNode), true);

                if (GUILayout.Button("Remove Edge"))
                {
                    Undo.RecordObject(node, "Remove Edge");
                    NavGraphNode neighbor = node.Neighbors[i];
                    node.RemoveUndirectNeighbor(neighbor);

                    if (neighbor != null)
                    {
                        Undo.RecordObject(neighbor, "Remove Edge");
                        neighbor.RemoveUndirectNeighbor(node);
                    }
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUI.changed) EditorUtility.SetDirty(node);
    }
}