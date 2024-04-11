#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

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

#endif