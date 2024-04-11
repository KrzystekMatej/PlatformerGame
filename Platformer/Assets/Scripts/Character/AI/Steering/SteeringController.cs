using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Events;

public class SteeringController : MonoBehaviour
{
    public SteeringPipeline CurrentPipeline { get; private set; }
    public ProcessState State { get; private set; } = ProcessState.Failure;

    public UnityEvent<SteeringPipeline, SteeringPipeline> OnPipelineSwitch;

    private AIInputController inputController;


    private void Awake()
    {
        inputController = GetComponentInParent<AIInputController>();
    }

    public void WritePipelinesToBlackboard(Blackboard blackboard)
    {
        SteeringPipeline[] pipelines = GetComponentsInChildren<SteeringPipeline>();
        foreach (SteeringPipeline pipeline in pipelines)
        {
            blackboard.SetValue(pipeline.name, pipeline);
            pipeline.WriteComponentsToBlackboard(blackboard);
        }
    }

    public void ApplySteering()
    {
        if (CurrentPipeline == null) return;

        (ProcessState state, Vector2 force) = CurrentPipeline.GetSteering();
        State = state;

        if (state != ProcessState.Running)
        {
            UpdateCurrentPipeline(null);
        }
        else inputController.AddSteeringForce(force);
    }

    public bool UpdateCurrentPipeline(SteeringPipeline newPipeline)
    {
        SteeringPipeline previous = CurrentPipeline;
        if (CurrentPipeline)
        {
            if (newPipeline == CurrentPipeline) return false;
            CurrentPipeline.Disable();
#if UNITY_EDITOR
            Debug.Log($"{CurrentPipeline.name} Deactivated");
#endif
        }
        else if(!newPipeline) return false;

        if (newPipeline)
        {
            CurrentPipeline = newPipeline;
            CurrentPipeline.Enable();
            State = ProcessState.Running;
#if UNITY_EDITOR
            Debug.Log($"{CurrentPipeline.name} Activated");
#endif
        }
        else
        {
            CurrentPipeline = null;
            if (State == ProcessState.Running) State = ProcessState.Failure;
        }

        OnPipelineSwitch?.Invoke(previous, CurrentPipeline);
        return true;
    }

    public bool IsRunning()
    {
        return CurrentPipeline != null;
    }
}