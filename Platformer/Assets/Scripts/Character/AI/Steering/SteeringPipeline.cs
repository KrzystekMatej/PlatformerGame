using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class SteeringPipeline : MonoBehaviour
{
    [field: SerializeField]
    public string PipelineName { get; private set; }
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

    public void Disable()
    {
        foreach (Targeter t in targeters) t.Enable();
        foreach (Decomposer d in decomposers) d.Enable();
        foreach (Constraint c in constraints) c.Enable();
        actuator.Enable();
    }

    public void Enable()
    {
        foreach (Targeter t in targeters) t.Disable();
        foreach (Decomposer d in decomposers) d.Disable();
        foreach (Constraint c in constraints) c.Disable();
        actuator.Disable();
    }

    public void BindBlackboard(Blackboard blackboard)
    {
        foreach (Targeter t in targeters) blackboard.SetValue(PipelineName + t.GetType().Name, t);
        foreach (Decomposer d in decomposers) blackboard.SetValue(PipelineName + d.GetType().Name, d);
        foreach (Constraint c in constraints) blackboard.SetValue(PipelineName + c.GetType().Name, c);
        blackboard.SetValue(PipelineName + actuator.GetType().Name, actuator);
    }

    public (ProcessState, Vector2) GetSteering()
    {
        SteeringGoal goal = new SteeringGoal();
        
        foreach (Targeter targeter in targeters)
        {
            if (targeter.TryUpdateGoal(goal)) return (ProcessState.Success, Vector2.zero);
        }

        if (goal.HasNothing()) return (ProcessState.Failure, Vector2.zero);

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
                if (steeringForce != null) return (ProcessState.Running, (Vector2)steeringForce);
                else break;
            }
        }

        return (ProcessState.Failure, Vector2.zero);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || gizmoGoalPosition == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((Vector3)gizmoGoalPosition, 0.3f);
    }
}
