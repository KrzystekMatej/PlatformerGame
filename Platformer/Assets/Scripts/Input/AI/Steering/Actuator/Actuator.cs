using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actuator : MonoBehaviour
{
    public abstract List<Vector2> GetPath(AgentManager agent, SteeringGoal goal);
    public abstract Vector2? GetSteering(AgentManager agent, List<Vector2> pointPath, SteeringGoal goal);
}