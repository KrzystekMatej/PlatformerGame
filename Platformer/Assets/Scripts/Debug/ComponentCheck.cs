using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class ComponentCheck : MonoBehaviour
{
    [SerializeField]
    private string componentName;

    public void CheckComponent()
    {
        if (string.IsNullOrEmpty(componentName))
        {
            Debug.LogError("Component name is not specified.");
            return;
        }
    
        Type type = Type.GetType(componentName);
        if (type == null)
        {
            Debug.LogError($"Component type '{componentName}' not found.");
            return;
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            var component = obj.GetComponent(type);
            if (component != null)
            {
                Debug.Log($"Object with name '{obj.name}' has the component '{type}'.");
            }
        }
    }
}

[CustomEditor(typeof(ComponentCheck))]
public class MonoBehaviourCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        ComponentCheck script = (ComponentCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check for Component"))
        {
            script.CheckComponent();
        }
    }
}

#endif
