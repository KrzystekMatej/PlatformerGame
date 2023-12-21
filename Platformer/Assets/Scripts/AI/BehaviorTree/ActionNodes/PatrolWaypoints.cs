using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PatrolWaypoints : ActionNode
{
    protected override State OnUpdate()
    {


        return State.Running;
    }
}
