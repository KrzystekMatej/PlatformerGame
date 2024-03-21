using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public Sound ActivationSound;
    public BackgroundController[] BackgroundControllers { get; private set; }

    private void Awake()
    {
        BackgroundControllers = FindObjectsOfType<BackgroundController>();
    }

    private void Start()
    {
        foreach (BackgroundController controller in BackgroundControllers)
        { 
            controller.CacheBackgroundData();
        }
    }
}
