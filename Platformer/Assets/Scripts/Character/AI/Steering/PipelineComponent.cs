using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
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
