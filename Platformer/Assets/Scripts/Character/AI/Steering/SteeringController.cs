using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Events;

public class SteeringController : MonoBehaviour
{
    [field: SerializeField]
    public SteeringPipeline CurrentPipeline { get; private set; }

    public UnityEvent<SteeringPipeline, SteeringPipeline> OnPipelineSwitch;

    private AIInputController inputController;
    private Vector2 currentForce;


    private void Awake()
    {
        inputController = GetComponentInParent<AIInputController>();
    }

    public void InitializePipelines()
    {
        SteeringPipeline[] pipelines = GetComponentsInChildren<SteeringPipeline>();
        foreach (SteeringPipeline p in pipelines)
        {
            p.enabled = false;
        }
        CurrentPipeline.Enable();
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

    public ProcessState Recalculate()
    {
        SteeringOutput output = CurrentPipeline.GetSteering();
        currentForce = output.Force;
        return output.State;
    }

    public void Apply()
    {
        inputController.AddSteeringForce(currentForce);
        ResetForce();
    }

    public void ResetForce()
    {
        currentForce = Vector2.zero;
    }

    public bool SwitchPipeline(SteeringPipeline newPipeline)
    {
        if (!newPipeline || newPipeline == CurrentPipeline) return false;
#if UNITY_EDITOR
        Debug.Log($"{CurrentPipeline.name} Deactivated");
        Debug.Log($"{newPipeline.name} Activated");
#endif

        OnPipelineSwitch?.Invoke(CurrentPipeline, newPipeline);
        CurrentPipeline.Disable();
        CurrentPipeline = newPipeline;
        CurrentPipeline.Enable();
        return true;
    }

    public bool IsRunning()
    {
        return CurrentPipeline;
    }
}