#if UNITY_EDITOR

using System;
using UnityEngine;

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
        int componentCount = 0;
        foreach (GameObject obj in allObjects)
        {
            var component = obj.GetComponent(type);
            if (component)
            {
                Debug.Log($"Object with name '{obj.name}' has the component '{type}'.");
                componentCount++;
            }
        }

        Debug.Log($"{componentCount} objects with '{type}' components found.");
    }
}

#endif
