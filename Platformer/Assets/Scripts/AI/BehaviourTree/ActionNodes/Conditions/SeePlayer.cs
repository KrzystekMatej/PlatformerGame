using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : Condition
{
    protected override bool IsConditionSatisfied()
    {
        float orientation = context.Agent.OrientationController.CurrentOrientation;
        CastDetector detector = (CastDetector)context.Vision.GetDetector(orientation == 1 ? "Right" : "Left");
        RaycastHit2D hit = detector.Hit;
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}
