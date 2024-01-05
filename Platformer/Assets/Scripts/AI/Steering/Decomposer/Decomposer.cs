using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Decomposer
{
    SteeringGoal Decompose(Agent agent, SteeringGoal goal);
}
