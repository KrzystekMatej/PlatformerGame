using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [field: SerializeField]
    public float TimeToDestroy { get; set; } = 0;

    public void Destroy()
    {
        if (TimeToDestroy > 0) StartCoroutine(WaitAndDestroy());
        else Destroy(gameObject);
    }

    public IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(TimeToDestroy);
        Destroy(gameObject);
    }
}
