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
    [SerializeField]
    private SteeringPipeline deadlock;
    private int constraintSteps;

#if UNITY_EDITOR
    Vector2 gizmoGoalPosition;
#endif

    private void Awake()
    {
        constraintSteps = constraints.Length + 1;
    }

    public void BindBlackboard(Blackboard blackboard)
    {
        foreach (Targeter targeter in targeters)
        {
            blackboard.DataTable[PipelineName + targeter.GetType().Name] = targeter;
        }

        foreach (Decomposer decomposer in decomposers)
        {
            blackboard.DataTable[PipelineName + decomposer.GetType().Name] = decomposer;
        }

        foreach (Constraint constraint in constraints)
        {
            blackboard.DataTable[PipelineName + constraint.GetType().Name] = constraint;
        }

        blackboard.DataTable[PipelineName + actuator.GetType().Name] = actuator;
    }

    public Vector2? GetSteering(AgentManager agent)
    {
        SteeringGoal goal = new SteeringGoal();

        foreach (Targeter targeter in targeters)
        {
            if (!targeter.TryUpdateGoal(agent, goal)) return null;
        }

        if (goal.HasNothing()) return GetDeadlockSteering(agent);

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
            gizmoGoalPosition = goal.Position;
#endif
            if (validPath)
            {
                Vector2? steeringForce = actuator.GetSteering(agent, path, goal);
                if (steeringForce != null) return steeringForce;
                else break;
            }

        }

        return GetDeadlockSteering(agent);
    }

    private Vector2? GetDeadlockSteering(AgentManager agent)
    {
        return deadlock != null ? deadlock.GetSteering(agent) : null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.3f);
    }
}
