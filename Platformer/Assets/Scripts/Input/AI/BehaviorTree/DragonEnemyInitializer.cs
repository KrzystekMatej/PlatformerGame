using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;

public class DragonEnemyInitializer : BehaviourTreeInitializer
{
    [SerializeField]
    private GameObject[] waypoints;

    public override void Initialize(BehaviourTree behaviourTree, Context context)
    {
        behaviourTree.blackboard.DataTable["Targets"] = waypoints.Select(w => w.transform.position).ToArray();
    }
}
