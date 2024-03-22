using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR

public class SortingLayerCheck : MonoBehaviour
{
    [SerializeField]
    private string testSortingLayerName;

    public void CheckLayer()
    {
        Renderer[] allSpriteRenderers = FindObjectsOfType<Renderer>();
        bool found = false;

        foreach (Renderer renderer in allSpriteRenderers)
        {
            if (renderer.sortingLayerName == testSortingLayerName)
            {
                Debug.Log($"Found a SpriteRenderer on {testSortingLayerName} sorting layer in GameObject named {renderer.gameObject.name}.");
                found = true;
            }
        }

        if (!found)
        {
            Debug.Log($"No SpriteRenderer found on {testSortingLayerName} sorting layer.");

        }
    }
}

[CustomEditor(typeof(SortingLayerCheck))]
public class SortingLayerCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SortingLayerCheck script = (SortingLayerCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check Layer"))
        {
            script.CheckLayer();
        }
    }
}

#endif
