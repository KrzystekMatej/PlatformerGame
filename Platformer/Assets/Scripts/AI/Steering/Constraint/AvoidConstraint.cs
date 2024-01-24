using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AvoidConstraint : Constraint
{
    [SerializeField]
    protected LayerMask avoidLayerMask;
    [SerializeField]
    protected int maxObstacleCount;

    protected RaycastHit2D[] hits;
    protected int hitCount;

    private void Awake()
    {
        hits = new RaycastHit2D[maxObstacleCount];
    }
}
