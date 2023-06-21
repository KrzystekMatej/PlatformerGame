using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : Condition
{
    protected override bool IsConditionSatisfied()
    {
        float orientation = context.agent.OrientationController.CurrentOrientation;
        RaycastHit2D hit = context.vision.GetRaycastHit(orientation == 1 ? "Right" : "Left");
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}
