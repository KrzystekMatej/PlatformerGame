using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Constraint : MonoBehaviour
{
    public abstract bool IsViolated(Agent agent, List<Vector2> pointPath);
    public abstract SteeringGoal Suggest(Agent agent, List<Vector2> pointPath, SteeringGoal goal);
}
