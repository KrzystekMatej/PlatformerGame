using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CheckHit : ConditionNode
{
    [SerializeField]
    private NodeProperty<RaycastHit2D> toCheck;
    [SerializeField]
    private float minDistance = 0;
    [SerializeField]
    private float maxDistance = float.PositiveInfinity;
    [SerializeField]
    private LayerMask mask;



    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        return (toCheck.Value.distance >= minDistance)
        && (toCheck.Value.distance <= maxDistance)
           && (mask == 0 || Utility.CheckLayer(toCheck.Value.collider.gameObject.layer, mask));
    }

    protected override void OnStop() { }
}
