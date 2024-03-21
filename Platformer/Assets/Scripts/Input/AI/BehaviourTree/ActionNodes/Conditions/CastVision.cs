using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastVision : Condition
{
    [SerializeField]
    private CastDetector visionDetector;
    [SerializeField]
    private float distance;
    [SerializeField]
    private bool changeDirectionByOrientation;
    [SerializeField]
    private bool changeOffsetByOrientation;

    protected override void OnStart()
    {
        visionDetector.Distance = distance;

        float orientation = context.Agent.OrientationController.CurrentOrientation;
        if (changeDirectionByOrientation)
        {
            visionDetector.Direction = new Vector2(orientation, 0);
        }
        if (changeOffsetByOrientation)
        {
            float xOffset = Mathf.Abs(visionDetector.OriginOffset.x) * orientation;
            visionDetector.OriginOffset = new Vector2(xOffset, visionDetector.OriginOffset.y);
        }
    }

    protected override bool IsConditionSatisfied()
    {
        int detectionCount = visionDetector.Detect(context.Agent.CenterPosition);
        blackboard.SetValue(visionDetector.name + "VisionHits", visionDetector.Hits);
        blackboard.SetValue(visionDetector.name + "VisionCount", detectionCount);
        return detectionCount > 0;
    }

    protected override void OnStop() { }
}
