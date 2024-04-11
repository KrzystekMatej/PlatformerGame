using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class ListGet : ActionNode
{
    [SerializeField]
    private NodeProperty<int> index;
    [SerializeField]
    private NodeProperty<IList> list;
    [SerializeField]
    private NodeProperty<object> item;


    protected override void OnStart() { }

    protected override ProcessState OnUpdate()
    {
        if (index.Value < list.Value.Count)
        {
            item.Value = (list.Value[index.Value]);
            return ProcessState.Success;
        }
        return ProcessState.Failure;
    }

    protected override void OnStop() { }
}
