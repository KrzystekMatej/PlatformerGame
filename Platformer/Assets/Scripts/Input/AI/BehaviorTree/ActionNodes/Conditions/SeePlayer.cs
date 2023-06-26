using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : Condition
{
    protected override bool IsConditionSatisfied()
    {
        float orientation = context.Agent.OrientationController.CurrentOrientation;
        RaycastHit2D hit = context.RayCastDetector.GetVisionRay(orientation == 1 ? "Right" : "Left").hit;
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}
