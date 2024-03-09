using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decomposer : MonoBehaviour
{
    public abstract SteeringGoal Decompose(AgentManager agent, SteeringGoal goal);
}
