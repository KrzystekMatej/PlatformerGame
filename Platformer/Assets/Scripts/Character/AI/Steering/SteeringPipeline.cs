using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;

public class SteeringPipeline : MonoBehaviour
{
    [SerializeField]
    private Targeter[] targeters;
    [SerializeField]
    private Decomposer[] decomposers;
    [SerializeField]
    private Constraint[] constraints;
    [SerializeField]
    private Actuator actuator;
    private int constraintSteps;

    private void Awake()
    {
        constraintSteps = constraints.Length + 1;
    }

    public SteeringOutput GetSteering()
    {
        SteeringGoal goal = new SteeringGoal();
        
        foreach (Targeter targeter in targeters)
        {
            switch (targeter.TryUpdateGoal(goal))
            {
                case ProcessState.Success: 
                    return SteeringOutput.Success;
                case ProcessState.Failure: 
                    return SteeringOutput.Failure;
            };
        }

        foreach (Decomposer decomposer in decomposers)
        {
            goal = decomposer.Decompose(goal);
        }

        for (int i = 0; i < constraintSteps; i++)
        {
            List<Vector2> path = actuator.GetPath(goal);
            bool validPath = true;

            foreach (Constraint constraint in constraints)
            {
                if (constraint.IsViolated(path))
                {
                    goal = constraint.Suggest(path, goal);
                    validPath = false;
                    break;
                }
            }

            if (validPath)
            {
                return actuator.GetSteering(path, goal);
            }
        }

        return SteeringOutput.Failure;
    }

    public void Enable()
    {
        enabled = true;
        foreach (PipelineComponent c in GetAllComponents()) c.Enable();
    }

    public void Disable()
    {
        enabled = false;
        foreach (PipelineComponent c in GetAllComponents()) c.Disable();
    }

    public void WriteComponentsToBlackboard(Blackboard blackboard)
    {
        foreach (PipelineComponent c in GetAllComponents()) c.WriteToCorrespondingKeys(blackboard);
    }

    private IEnumerable<PipelineComponent> GetAllComponents()
    {
        return targeters
            .Select(t => (PipelineComponent)t)
            .Concat(decomposers)
            .Concat(constraints)
            .Append(actuator);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || !enabled) return;
        foreach (PipelineComponent c in GetAllComponents()) c.DrawGizmos();
    }
#endif
}
