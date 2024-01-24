using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : Condition
{
    [SerializeField]
    float groundDistance = 1f;

    [SerializeField]
    private CastDetector groundCheckRight;
    [SerializeField]
    private CastDetector groundCheckLeft;

    protected override bool IsConditionSatisfied()
    {
        CastDetector detector = (float)blackboard.DataTable["HorizontalDirection"] == 1f ? groundCheckRight : groundCheckLeft;
        int detectionCount = detector.Detect(context.Agent.CenterPosition);
        return detectionCount == 0 || detector.Hits[0].distance > groundDistance;
    }
}
