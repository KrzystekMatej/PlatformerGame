using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public string CurrentPipeline { get; set; }
    
    private Dictionary<string, SteeringPipeline> pipelineTable = new Dictionary<string, SteeringPipeline>();

    private void Awake()
    {
        foreach (SteeringPipeline pipeline in GetComponents<SteeringPipeline>())
        {
            pipelineTable[pipeline.PipelineName] = pipeline;
        }
    }

    public void ApplySteering(Agent agent, AIInputController inputController)
    {
        if (CurrentPipeline != null)
        {
            inputController.SetMovementVector(pipelineTable[CurrentPipeline].GetSteering(agent));
        }
    }
}
