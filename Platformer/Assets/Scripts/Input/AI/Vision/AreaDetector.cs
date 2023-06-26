using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaDetector : VisionDetector
{
    [field: SerializeField]
    public float DetectionRadius { get; protected set; }
    [SerializeField]
    protected int maxCollidersToDetect;
    [SerializeField]
    protected LayerMask detectLayerMask;
    protected Collider2D[] colliders;
    public int ColliderCount { get; protected set; }

    private void Awake()
    {
        colliders = new Collider2D[maxCollidersToDetect];
    }

    public Collider2D[] GetColliders()
    {
        return colliders;
    }
}
