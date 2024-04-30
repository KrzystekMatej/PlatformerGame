using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public abstract class PipelineComponent : MonoBehaviour
{
    public AgentManager Agent { get; private set; }

    protected virtual void Awake()
    {
        Agent = GetComponentInParent<AIInputController>().GetComponentInChildren<AgentManager>();
    }

    public virtual void Enable() { }
    public virtual void Disable() { }

    public void WriteToCorrespondingKeys(Blackboard blackboard)
    {
        foreach (BlackboardKey key in blackboard.keys)
        {
            bool keyIsBaseComponent = typeof(PipelineComponent).IsAssignableFrom(key.underlyingType);
            bool concreteComponentIsKey = key.underlyingType.IsAssignableFrom(GetType());
            if (keyIsBaseComponent && concreteComponentIsKey && key.name.Contains(name))
            {
                key.SetValue(this);
            }
        }
    }
#if UNITY_EDITOR
    public virtual void DrawGizmos() { }
#endif
}