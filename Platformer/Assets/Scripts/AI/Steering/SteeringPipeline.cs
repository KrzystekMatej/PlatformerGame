using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
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

    private void Awake()
    {
        targeters = GetComponents<Targeter>();
        decomposers = GetComponents<Decomposer>();
        constraints = GetComponents<Constraint>();
        actuator = GetComponent<Actuator>();


        constraintSteps = constraints.Length == 0 ? 1 : constraints.Length;
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
                if (constraint.IsViolated(path))
                {
                    goal = constraint.Suggest(agent, path, goal);
                    validPath = false;
                    break;
                }
            }

            if (validPath)
            {
                return actuator.GetSteering(agent, path, goal);
            }
        }

        return deadlock.GetSteering(agent);
    }
}
