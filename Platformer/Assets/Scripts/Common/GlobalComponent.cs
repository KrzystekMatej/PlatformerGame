using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalComponent<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
#if UNITY_EDITOR
    private void Start()
    {
        var instances = FindObjectsOfType<T>();
        if (instances.Count() > 1) Debug.LogWarning("Multiple instances of global component present in current scene.");
        //float shift = objectCollider.transform.position.x - transform.position.x;
        //transform.position = new Vector2(transform.position.x - shift * 2, transform.position.y);
    }
#endif
}
