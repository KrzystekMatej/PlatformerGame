using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckVision : ConditionNode
{
    [SerializeField]
    private CastDetector visionDetector;
    [SerializeField]
    private int minCount = 1;
    [SerializeField]
    private int maxCount = int.MaxValue;
    [SerializeField]
    private float minDistance = 0;
    [SerializeField]
    private float maxDistance = float.PositiveInfinity;
    [SerializeField]
    private LayerMask mask;

#if UNITY_EDITOR
    public override void OnInit()
    {
        if (minCount > maxCount)
        {
            Log(LogType.Warning, "Min count is bigger than max count.");
        }
        if (minDistance > maxDistance)
        {
            Log(LogType.Warning, "Min distance is bigger than max distance.");
        }
    }
#endif


    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        RaycastHit2D[] hits = blackboard.GetValue<RaycastHit2D[]>(visionDetector.name + "VisionHits");
        int visionCount = blackboard.GetValue<int>(visionDetector.name + "VisionCount");
        List<RaycastHit2D> checkedHits = new List<RaycastHit2D>();
        int count = 0;
        for (int i = 0; i < visionCount; i++)
        {
            if (CheckHit(hits[i]))
            {
                count++;

                if (count > maxCount) break;
                checkedHits.Add(hits[i]);
            }
        }

        if (count >= minCount && count <= maxCount)
        {
            blackboard.SetValue("CheckedHits", checkedHits);
            return true;
        }
        return false;
    }

    protected override void OnStop() { }

    private bool CheckHit(RaycastHit2D hit)
    {
        return (hit.distance >= minDistance)
            && (hit.distance <= maxDistance)
            && (mask == 0 || Utility.CheckLayer(hit.collider.gameObject.layer, mask));
    }
}
