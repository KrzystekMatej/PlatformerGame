using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : Condition
{
    [SerializeField]
    private CastDetector right;
    [SerializeField]
    private CastDetector left;

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        float orientation = context.Agent.OrientationController.CurrentOrientation;
        CastDetector detector = orientation == 1 ? right : left;
        int detectionCount = detector.Detect(context.Agent.CenterPosition);
        return detectionCount != 0 && detector.Hits[0].collider.CompareTag("Player");
    }

    protected override void OnStop() { }
}
