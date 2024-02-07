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
    private Targeter[] targeters;
    private Decomposer[] decomposers;
    private Constraint[] constraints;
    private Actuator actuator;
    [SerializeField]
    private SteeringPipeline deadlock;
    private int constraintSteps;

#if UNITY_EDITOR
    Vector2 gizmoGoalPosition;
#endif

    private void Awake()
    {
        targeters = GetComponents<Targeter>();
        decomposers = GetComponents<Decomposer>();
        constraints = GetComponents<Constraint>();
        actuator = GetComponent<Actuator>();


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

    public Vector2 GetSteering(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();

        foreach (Targeter targeter in targeters)
        {
            goal.UpdateChannels(targeter.GetGoal(agent));
        }

        if (goal.HasNothing()) return Vector2.zero;

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
                return actuator.GetSteering(agent, path, goal);
            }

        }

        return deadlock != null ? deadlock.GetSteering(agent) : Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gizmoGoalPosition, 0.3f);
    }
}
