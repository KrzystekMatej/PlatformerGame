using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public BackgroundController backgroundController { get; private set; }

    private void Awake()
    {
        backgroundController = FindObjectOfType<BackgroundController>();
    }
}
