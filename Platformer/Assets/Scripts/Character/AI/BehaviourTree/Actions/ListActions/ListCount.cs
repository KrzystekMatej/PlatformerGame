using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ListCount : ActionNode
{
    [SerializeField]
    private NodeProperty<IList> list;
    [SerializeField]
    private NodeProperty<int> count;


    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        count.Value = list.Value.Count;
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}
