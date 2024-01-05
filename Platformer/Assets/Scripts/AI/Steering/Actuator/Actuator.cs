using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Actuator
{
    Path GetPath(Agent agent, SteeringGoal goal);
    Vector2 GetOutput(Agent agent, Path path, SteeringGoal goal);
}
