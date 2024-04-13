using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Events;

public class SteeringController : MonoBehaviour
{
    [field: SerializeField]
    public SteeringPipeline CurrentPipeline { get; private set; }

    public UnityEvent<SteeringPipeline, SteeringPipeline> OnPipelineSwitch;

    private AIInputController inputController;
    private Vector2 steeringForce;


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

    public ProcessState RecalculateSteering()
    {
        (ProcessState state, Vector2 force) = CurrentPipeline.GetSteering();
        steeringForce = force;
        return state;
    }

    public void ApplySteering()
    {
        if (steeringForce == Vector2.zero) return;
        inputController.AddSteeringForce(steeringForce);
        steeringForce = Vector2.zero;
    }

    public bool UpdateCurrentPipeline(SteeringPipeline newPipeline)
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