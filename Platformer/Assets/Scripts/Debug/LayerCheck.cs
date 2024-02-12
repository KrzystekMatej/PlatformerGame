using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class LayerCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask testLayerMask;

    public void CheckLayerInAllObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        bool found = false;
        foreach (GameObject obj in allObjects)
        {
            if (Utility.CheckLayer(obj.layer, testLayerMask))
            {
                Debug.Log("Object with name '" + obj.name + "' is on layer '" + LayerMask.LayerToName(obj.layer) + "'.");
                found = true;
            }
        }

        if (!found) Debug.Log("No object found on the specified layers.");
    }
}

[CustomEditor(typeof(LayerCheck))]
public class LayerCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LayerCheck script = (LayerCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check Layer"))
        {
            script.CheckLayerInAllObjects();
        }
    }
}

#endif
