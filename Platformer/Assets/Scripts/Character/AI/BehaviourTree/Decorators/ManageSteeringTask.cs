using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

[System.Serializable]
public class ManageSteeringTask : DecoratorNode
{
    [SerializeField]
    private NodeProperty<SteeringPipeline> newPipeline;

    private SteeringPipeline lastPipeline;
    private bool tryInterrupt;


    protected override void OnStart()
    {
        lastPipeline = context.Steering.CurrentPipeline;
        if (context.Steering.SwitchPipeline(newPipeline.Value))
        {
            newPipeline.Value.Enable();
            tryInterrupt = true;
        }
    }

    protected override ProcessState OnUpdate()
    {
        if (tryInterrupt)
        {
            ProcessState state = child.Update();
            if (state == ProcessState.Running)
            {
                lastPipeline.Disable();
            }
            tryInterrupt = false;
            return state;
        }
        return child.Update();
    }

    protected override void OnStop()
    {
        if (state != ProcessState.Running && context.Steering.SwitchPipeline(lastPipeline))
        {
            newPipeline.Value.Disable();
            lastPipeline.Enable();
        }
    }
}
