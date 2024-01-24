using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actuator : MonoBehaviour
{
    public abstract SteeringGoal GetFeasibleGoal(Agent agent, List<Vector2> pointPath, SteeringGoal goal);
    public abstract Vector2 GetSteering(Agent agent, List<Vector2> pointPath, SteeringGoal goal);
}
