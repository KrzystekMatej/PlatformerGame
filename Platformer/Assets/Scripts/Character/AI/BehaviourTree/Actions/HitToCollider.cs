using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class HitToCollider : ActionNode
{
    [SerializeField]
    private NodeProperty<RaycastHit2D> hit;
    [SerializeField]
    private NodeProperty<Collider2D> collider;



    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        collider.Value = hit.Value.collider;
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}
