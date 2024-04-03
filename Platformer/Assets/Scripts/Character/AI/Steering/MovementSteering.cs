using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Events;

public class MovementSteering : MonoBehaviour
{
    public UnityEvent<SteeringPipeline, SteeringPipeline> OnPipelineSwitch;


    private SteeringPipeline currentPipeline;

    private Dictionary<string, SteeringPipeline> pipelineTable = new Dictionary<string, SteeringPipeline>();

    public string CurrentPipelineName => currentPipeline != null ? currentPipeline.PipelineName : null;
    public ProcessState State { get; private set; }

    private AIInputController inputController;

    private void Awake()
    {
        SteeringPipeline[] pipelines = GetComponentsInChildren<SteeringPipeline>(true);
        foreach (SteeringPipeline p in pipelines)
        {
            pipelineTable[p.PipelineName] = p;
        }
        inputController = GetComponentInParent<AIInputController>();
    }

    public SteeringPipeline GetPipeline(string pipelineName)
    {
        return pipelineTable[pipelineName];
    }

    public void BindBlackboard(Blackboard blackboard)
    {
        foreach (SteeringPipeline pipeline in pipelineTable.Values)
        {
            pipeline.BindBlackboard(blackboard);
        }
    }

    public void ApplySteering()
    {
        if (currentPipeline == null) return;

        (ProcessState state, Vector2 force) = currentPipeline.GetSteering();
        State = state;

        if (state != ProcessState.Running)
        {
            UpdateCurrentPipeline(null);
        }
        else inputController.AddSteeringForce(force);
    }

    public bool UpdateCurrentPipeline(SteeringPipeline newPipeline)
    {
        SteeringPipeline previous = currentPipeline;
        if (currentPipeline != null)
        {
            if (newPipeline == currentPipeline) return false;
            currentPipeline.Disable();
            inputController.StopMoving();
#if UNITY_EDITOR
            Debug.Log("Deactivated");
#endif
        }
        else if(newPipeline == null) return false;

        if (newPipeline != null)
        {
            currentPipeline = newPipeline;
            currentPipeline.Enable();
            State = ProcessState.Running;
#if UNITY_EDITOR
            Debug.Log("Activated");
#endif
        }
        else currentPipeline = null;
        OnPipelineSwitch?.Invoke(previous, currentPipeline);
        return true;
    }

    public bool IsRunning()
    {
        return currentPipeline != null;
    }
}