using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Constraint : MonoBehaviour
{
    public abstract bool IsViolated(AgentManager agent, List<Vector2> pointPath);
    public abstract SteeringGoal Suggest(AgentManager agent, List<Vector2> pointPath, SteeringGoal goal);
}
