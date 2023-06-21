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
        RaycastHit2D groundHit = context.vision.GetRaycastHit((float)blackboard.DataTable["HorizontalDirection"] == 1 ? groundCheckRight : groundCheckLeft);
        return groundHit.collider == null || groundHit.distance > groundDistance;
    }
}
