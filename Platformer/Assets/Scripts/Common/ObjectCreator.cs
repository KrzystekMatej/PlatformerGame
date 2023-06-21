using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToCreate;
    [SerializeField]
    [Range(0f, 1f)]
    private float probability;

    public void Create()
    {
        if (Random.value <= probability)
        {
            Collider2D collider = GetComponent<Collider2D>();
            Instantiate(objectToCreate, collider.bounds.center, Quaternion.identity);
        }
    }
}
