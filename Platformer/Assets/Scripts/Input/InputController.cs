using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    protected InputData inputData;
    public InputData InputData { get => inputData; }
    public (bool x, bool y) DecelerationFlags;

    protected AgentInstanceData instanceData;

    protected void Start()
    {
        instanceData = GetComponentInChildren<AgentManager>().InstanceData;
    }
}
