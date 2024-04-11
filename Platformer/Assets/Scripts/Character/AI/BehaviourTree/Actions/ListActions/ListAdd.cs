using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ListAdd : ActionNode
{
    [SerializeField]
    private NodeProperty<IList> list;
    [SerializeField]
    private NodeProperty<object> toAdd;


    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        list.Value.Add(toAdd.Value);
        Debug.Log(toAdd.Value);
        return ProcessState.Success;
    }

    protected override void OnStop() { }
}
