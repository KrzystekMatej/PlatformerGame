#if UNITY_EDITOR

using UnityEngine;

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

#endif