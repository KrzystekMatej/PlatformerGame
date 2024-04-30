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
        SteeringPipeline[] pipelines = GetComponentsInChildren<SteeringPipeline>(true);
        foreach (SteeringPipeline p in pipelines)
        {
            p.enabled = false;
        }
    }

    public void WritePipelinesToBlackboard(Blackboard blackboard)
    {
        SteeringPipeline[] pipelines = GetComponentsInChildren<SteeringPipeline>(true);
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

    public void Restart()
    {
        currentForce = Vector2.zero;
    }

    public void Apply()
    {
        inputController.AddSteeringForce(currentForce);
        Restart();
    }

    public bool SwitchPipeline(SteeringPipeline newPipeline)
    {
        if (!newPipeline || newPipeline == CurrentPipeline) return false;
        OnPipelineSwitch?.Invoke(CurrentPipeline, newPipeline);
        CurrentPipeline = newPipeline;
        return true;
    }

    public bool IsRunning()
    {
        return CurrentPipeline;
    }
}