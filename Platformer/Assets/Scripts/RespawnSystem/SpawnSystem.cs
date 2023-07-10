using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public BackgroundController[] BackgroundControllers { get; private set; }

    private void Awake()
    {
        BackgroundControllers = FindObjectsOfType<BackgroundController>();
    }
}
