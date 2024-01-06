using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringPipeline : MonoBehaviour
{
    [SerializeField]
    private List<Targeter> targeters = new List<Targeter>();
    [SerializeField]
    private List<Decomposer> decomposers = new List<Decomposer>();
    [SerializeField]
    private List<Constraint> constraints = new List<Constraint>();
    [SerializeField]
    private Actuator actuator;
    private int constraintSteps;

    private void Awake()
    {
        constraintSteps = constraints.Count;
    }

    private SteeringPipeline deadlock;

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
            Path path = actuator.GetPath(agent, goal);

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
                return actuator.GetOutput(agent, path, goal);
            }
        }

        return deadlock.GetSteering(agent);
    }
}
