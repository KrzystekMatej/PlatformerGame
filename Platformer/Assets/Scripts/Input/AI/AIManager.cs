using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public Vision Vision { get; private set; }
    public Steering Steering { get; private set; }

    private void Awake()
    {
        Vision = GetComponentInChildren<Vision>();
        Steering = GetComponentInChildren<Steering>();
    }

    private void Start()
    {
        transform.position = GetComponentInParent<Agent>().TriggerCollider.bounds.center;
    }
}
