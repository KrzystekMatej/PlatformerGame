using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : Condition
{
    [SerializeField]
    float wallDistance = 1.3f;

    [SerializeField]
    private CastDetector wallCheckRight;
    [SerializeField]
    private CastDetector wallCheckLeft;

    protected override bool IsConditionSatisfied()
    {        
        CastDetector detector = (float)blackboard.DataTable["HorizontalDirection"] == 1f ? wallCheckRight : wallCheckLeft;
        int detectionCount = detector.Detect(context.Agent.CenterPosition);
        return detectionCount != 0 && detector.Hits[0].distance < wallDistance && detector.Hits[0].collider.GetComponent<Agent>() == null;
    }
}