using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ListClear : ActionNode
{
    [SerializeField]
    private NodeProperty<IList> list;

    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        list.Value.Clear();
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}
