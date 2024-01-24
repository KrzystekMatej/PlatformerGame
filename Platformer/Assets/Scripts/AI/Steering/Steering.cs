using System.Collections.Generic;
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

    public void BindBlackboard(Blackboard blackboard)
    {
        foreach (SteeringPipeline pipeline in pipelineTable.Values)
        {
            pipeline.BindBlackboard(blackboard);
            pipeline.gameObject.SetActive(false);
        }
    }

    public void ApplySteering(Agent agent, AIInputController inputController)
    {
        Vector2 steeringForce;
        if (currentPipeline != null)
        {
            steeringForce = currentPipeline.GetSteering(agent);
            if (steeringForce == Vector2.zero) UpdateCurrentPipeline(null);
        }
        else steeringForce = Vector2.zero;

        inputController.SetMovementVector(steeringForce);
    }

    public bool UpdateCurrentPipeline(string pipelineName)
    {
        if (currentPipeline != null)
        {
            if (pipelineName == currentPipeline.PipelineName) return false;
            currentPipeline.gameObject.SetActive(false);
            Debug.Log("Deactivated");
        }
        else if(pipelineName == null) return false;

        if (pipelineName != null)
        {
            currentPipeline = pipelineTable[pipelineName];
            currentPipeline.gameObject.SetActive(true);
            Debug.Log("Activated");
        }
        else currentPipeline = null;

        return true;
    }
}
