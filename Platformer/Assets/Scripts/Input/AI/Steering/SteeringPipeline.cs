using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using TMPro;
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
    Vector2 gizmoGoalPosition;
    bool gizmoValidPath;
    List<Vector2> gizmoPath;
#endif

    private void Awake()
    {
        constraintSteps = constraints.Length + 1;
    }

    public void BindBlackboard(Blackboard blackboard)
    {
        foreach (Targeter targeter in targeters)
        {
            blackboard.SetValue(PipelineName + targeter.GetType().Name, targeter);
        }

        foreach (Decomposer decomposer in decomposers)
        {
            blackboard.SetValue(PipelineName + decomposer.GetType().Name, decomposer);
        }

        foreach (Constraint constraint in constraints)
        {
            blackboard.SetValue(PipelineName + constraint.GetType().Name, constraint);
        }

        blackboard.SetValue(PipelineName + actuator.GetType().Name, actuator);
    }

    public (ProcessState, Vector2) GetSteering(AgentManager agent)
    {
        SteeringGoal goal = new SteeringGoal();
        
        foreach (Targeter targeter in targeters)
        {
            if (targeter.TryUpdateGoal(agent, goal)) return (ProcessState.Success, Vector2.zero);
        }

        if (goal.HasNothing()) return (ProcessState.Failure, Vector2.zero);

        foreach (Decomposer decomposer in decomposers)
        {
            goal = decomposer.Decompose(agent, goal);
        }

        for (int i = 0; i < constraintSteps; i++)
        {
            List<Vector2> path = actuator.GetPath(agent, goal);
            bool validPath = true;

            foreach (Constraint constraint in constraints)
            {
                if (constraint.IsViolated(agent, path))
                {
                    goal = constraint.Suggest(agent, path, goal);
                    validPath = false;
                    break;
                }
            }

#if UNITY_EDITOR
            gizmoValidPath = validPath;
            gizmoPath = path;
            gizmoGoalPosition = goal.Position;
#endif
            if (validPath)
            {
                Vector2? steeringForce = actuator.GetSteering(agent, path, goal);
                if (steeringForce != null) return (ProcessState.Running, (Vector2)steeringForce);
                else break;
            }

        }

        return (ProcessState.Failure, Vector2.zero);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.3f);
        if (!gizmoValidPath) return;
        for (int i = 0; i < gizmoPath.Count - 1; i++)
        {
            Gizmos.DrawLine(gizmoPath[i], gizmoPath[i + 1]);
        }
    }
}
