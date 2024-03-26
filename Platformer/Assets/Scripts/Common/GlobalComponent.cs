using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalComponent<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        var instances = FindObjectsOfType<T>();
        if (instances.Count() > 1) Debug.LogWarning("Multiple instances of global component present in current scene.");
#endif
        Instance = this as T;
    }
}
