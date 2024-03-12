using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TheKiwiCoder;
using UnityEngine;

public class Steering : MonoBehaviour
{
    private SteeringPipeline currentPipeline;

    public string CurrentPipelineName
    {
        get
        {
            if (currentPipeline != null)
            {
                return currentPipeline.PipelineName;
            }
            return null;
        }
    }
    
    private Dictionary<string, SteeringPipeline> pipelineTable = new Dictionary<string, SteeringPipeline>();

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

        Vector2? steeringForce = currentPipeline.GetSteering(agent);

        if (steeringForce == null)
        {
            UpdateCurrentPipeline(null);
            inputController.StopMoving();
        }
        else inputController.AddSteeringForce(steeringForce.Value);
    }

    public bool UpdateCurrentPipeline(SteeringPipeline newPipeline)
    {
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

        return true;
    }
}
