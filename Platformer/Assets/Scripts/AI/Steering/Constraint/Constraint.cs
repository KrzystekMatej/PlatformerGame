using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Constraint : MonoBehaviour
{
    public abstract bool IsViolated(List<Vector2> path);
    public abstract SteeringGoal Suggest(Agent agent, List<Vector2> path, SteeringGoal goal);
}
