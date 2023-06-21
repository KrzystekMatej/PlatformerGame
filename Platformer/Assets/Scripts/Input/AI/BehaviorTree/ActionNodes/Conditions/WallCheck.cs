using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : Condition
{
    [SerializeField]
    float wallDistance = 1.3f;

    private string wallCheckRight = "Right";
    private string wallCheckLeft = "Left";

    protected override bool IsConditionSatisfied()
    {
        RaycastHit2D wallHit = context.vision.GetRaycastHit((float)blackboard.DataTable["HorizontalDirection"] == 1 ? wallCheckRight : wallCheckLeft);
        
        return wallHit.collider != null && wallHit.distance < wallDistance && wallHit.collider.GetComponent<Agent>() == null;
    }
}