#if UNITY_EDITOR

using UnityEngine;

public class LayerMaskCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask testLayerMask;

    public void CheckLayer()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        bool found = false;
        foreach (GameObject obj in allObjects)
        {
            if (Utility.CheckLayer(obj.layer, testLayerMask))
            {
                Debug.Log($"Object with name '{obj.name}' is on layer '{LayerMask.LayerToName(obj.layer)}'.");
                found = true;
            }
        }

        if (!found) Debug.Log("No object found on the specified layers.");
    }
}

#endif