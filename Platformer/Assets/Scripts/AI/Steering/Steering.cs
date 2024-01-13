using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void Awake()
    {
        foreach (SteeringPipeline pipeline in GetComponentsInChildren<SteeringPipeline>())
        {
            pipeline.gameObject.SetActive(false);
            pipelineTable[pipeline.PipelineName] = pipeline;
        }
    }

    public void BindBlackboard(Blackboard blackboard)
    {
        foreach (SteeringPipeline pipeline in pipelineTable.Values)
        {
            pipeline.BindBlackboard(blackboard);
        }
    }

    public void ApplySteering(Agent agent, AIInputController inputController)
    {
        Vector2 steeringForce = (currentPipeline != null) ? currentPipeline.GetSteering(agent) : Vector2.zero;

        inputController.SetMovementVector(steeringForce);
    }

    public bool UpdateCurrentPipeline(string pipelineName)
    {
        if (currentPipeline != null)
        {
            if (pipelineName == currentPipeline.PipelineName) return false;
            currentPipeline.gameObject.SetActive(false);
        }

        if (pipelineName != null)
        {
            currentPipeline = pipelineTable[pipelineName];
            currentPipeline.gameObject.SetActive(true);
        }

        return true;
    }
}
