using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    protected InputData inputData;
    public InputData InputData { get => inputData; }
    public (bool x, bool y) DecelerationFlags;

    protected AgentInstanceData instanceData;

    private void Start()
    {
        instanceData = GetComponentInChildren<Agent>().InstanceData;
    }
}
