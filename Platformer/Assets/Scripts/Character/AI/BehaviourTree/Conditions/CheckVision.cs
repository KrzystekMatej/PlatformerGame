using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;

public class CheckVision : ConditionNode
{
    [SerializeField]
    private NodeProperty<List<RaycastHit2D>> originalHits;
    [SerializeField]
    private NodeProperty<List<RaycastHit2D>> checkedHits;
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


    public override void OnInit()
    {
#if UNITY_EDITOR
        if (minCount > maxCount)
        {
            Log(LogType.Warning, "Min count is bigger than max count.");
        }
        if (minDistance > maxDistance)
        {
            Log(LogType.Warning, "Min distance is bigger than max distance.");
        }
#endif
    }

    protected override bool IsConditionSatisfied()
    {
        int count = 0;
        checkedHits.Value.Clear();
        foreach (RaycastHit2D hit in originalHits.Value)
        {
            if (CheckHit(hit))
            {
                count++;

                if (count > maxCount) break;
                checkedHits.Value.Add(hit);
            }
        }

        if (count >= minCount && count <= maxCount)
        {
            return true;
        }
        checkedHits.Value.Clear();
        return false;
    }

    private bool CheckHit(RaycastHit2D hit)
    {
        return (hit.distance >= minDistance)
            && (hit.distance <= maxDistance)
            && (mask == 0 || Utility.CheckLayer(hit.collider.gameObject.layer, mask));
    }
}
