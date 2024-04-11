#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialCheck : MonoBehaviour
{
    [SerializeField]
    private List<PhysicsMaterial2D> testMaterials;

    public void CheckMaterials()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        bool found = false;
        foreach (GameObject obj in allObjects)
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            Rigidbody2D rigidBody = obj.GetComponent<Rigidbody2D>();
            if (collider != null && testMaterials.Any(m => collider.sharedMaterial == m))
            {
                Debug.Log($"Object with name '{obj.name}' has physic material named '{collider.sharedMaterial.name}' on Collider2D component.");
                found = true;
            }

            if (rigidBody != null && testMaterials.Any(m => rigidBody.sharedMaterial == m))
            {
                Debug.Log($"Object with name '{obj.name}' has physic material named '{rigidBody.sharedMaterial.name}' on Rigidbody component.");
                found = true;
            }
        }

        if (!found) Debug.Log("No object found with the specified materials.");
    }
}

#endif