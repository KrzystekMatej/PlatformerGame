using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PipelineComponent : MonoBehaviour
{
    protected AgentManager agent;

    protected virtual void Start()
    {
        agent = GetComponentInParent<AIManager>().Agent;
    }

    public virtual void Enable() { }
    public virtual void Disable() { }
}
