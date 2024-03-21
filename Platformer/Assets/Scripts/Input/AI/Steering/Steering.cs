using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Events;

public class Steering : MonoBehaviour
{
    public UnityEvent<SteeringPipeline, SteeringPipeline> OnPipelineSwitch;


    private SteeringPipeline currentPipeline;

    private Dictionary<string, SteeringPipeline> pipelineTable = new Dictionary<string, SteeringPipeline>();

    public string CurrentPipelineName => currentPipeline != null ? currentPipeline.PipelineName : null;
    public ProcessState State { get; private set; }

    private void Awake()
    {
        foreach (SteeringPipeline pipeline in GetComponentsInChildren<SteeringPipeline>())
        {
            pipelineTable[pipeline.PipelineName] = pipeline;
        }
    }

    public SteeringPipeline GetPipeline(string pipelineName)
    {
        return pipelineTable[pipelineName];
    }

    public void Bind(Blackboard blackboard)
    {
        foreach (SteeringPipeline pipeline in pipelineTable.Values)
        {
            pipeline.BindBlackboard(blackboard);
            pipeline.gameObject.SetActive(false);
        }
    }

    public void ApplySteering(AgentManager agent, AIInputController inputController)
    {
        if (currentPipeline == null) return;

        (ProcessState state, Vector2 force) = currentPipeline.GetSteering(agent);
        State = state;

        if (state != ProcessState.Running)
        {
            UpdateCurrentPipeline(null);
            inputController.StopMoving();
        }
        else inputController.AddSteeringForce(force);
    }

    public bool UpdateCurrentPipeline(SteeringPipeline newPipeline)
    {
        SteeringPipeline previous = currentPipeline;
        if (currentPipeline != null)
        {
            if (newPipeline == currentPipeline) return false;
            currentPipeline.gameObject.SetActive(false);
#if UNITY_EDITOR
            Debug.Log("Deactivated");
#endif
        }
        else if(newPipeline == null) return false;

        if (newPipeline != null)
        {
            currentPipeline = newPipeline;
            currentPipeline.gameObject.SetActive(true);
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