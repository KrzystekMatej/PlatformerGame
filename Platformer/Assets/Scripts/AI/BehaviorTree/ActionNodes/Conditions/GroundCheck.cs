using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : Condition
{
    [SerializeField]
    float groundDistance = 1f;

    private string groundCheckRight = "RightDown";
    private string groundCheckLeft = "LeftDown";

    protected override bool IsConditionSatisfied()
    {
        CastDetector detector = (CastDetector)context.AIManager.Vision.GetDetector((float)blackboard.DataTable["HorizontalDirection"] == 1 ? groundCheckRight : groundCheckLeft);
        RaycastHit2D groundHit = detector.Hit;
        return groundHit.collider == null || groundHit.distance > groundDistance;
    }
}
