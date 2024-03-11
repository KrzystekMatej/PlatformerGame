using FibonacciHeap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HeapNode = FibonacciHeap.FibonacciHeapNode<NavGraphNode, float>;

public class NavGraphNode : MonoBehaviour
{
    [HideInInspector]
    public int Index;
    [HideInInspector]
    public List<NavGraphNode> Neighbors = new List<NavGraphNode>();
    [HideInInspector]
    public Vector2 ExpansionVector;

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

    public void AddNeighbor(NavGraphNode neighbor)
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


    public void RemoveNeighbor(NavGraphNode neighbor)
    {
        if (Neighbors.Remove(neighbor)) neighbor.Neighbors.Remove(this);
    }

    public Vector2 GetExpandedPosition(float expansionDistance)
    {
        return transform.position + (Vector3)ExpansionVector * expansionDistance;
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
            node.AddNeighbor(newNeighbor);
            Undo.RecordObject(newNeighbor, "Add Edge");
            newNeighbor.AddNeighbor(node);

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
                    node.RemoveNeighbor(neighbor);

                    if (neighbor != null)
                    {
                        Undo.RecordObject(neighbor, "Remove Edge");
                        neighbor.RemoveNeighbor(node);
                    }
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUI.changed) EditorUtility.SetDirty(node);
    }
}