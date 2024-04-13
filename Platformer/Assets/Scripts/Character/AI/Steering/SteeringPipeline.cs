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

#if UNITY_EDITOR
    Vector2? gizmoGoalPosition;
#endif

    private void Awake()
    {
        constraintSteps = constraints.Length + 1;
    }

    public (ProcessState, Vector2) GetSteering()
    {
        SteeringGoal goal = new SteeringGoal();
        
        foreach (Targeter targeter in targeters)
        {
            ProcessState targeterState = targeter.TryUpdateGoal(goal);
            if (targeterState != ProcessState.Running) return (targeterState, Vector2.zero);
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

#if UNITY_EDITOR
            gizmoGoalPosition = goal.HasPosition ? goal.Position : null;
#endif
            if (validPath)
            {
                Vector2? steeringForce = actuator.GetSteering(path, goal);
                if (steeringForce.HasValue) return (ProcessState.Running, steeringForce.Value);
                else break;
            }
        }

        return (ProcessState.Failure, Vector2.zero);
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
        if (!Application.isPlaying || gizmoGoalPosition == null) return;
        foreach (PipelineComponent c in GetAllComponents()) c.DrawGizmos();
    }
#endif
}
