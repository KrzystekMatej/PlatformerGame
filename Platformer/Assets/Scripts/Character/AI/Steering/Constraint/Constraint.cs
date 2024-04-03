using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Constraint : PipelineComponent
{
    public abstract bool IsViolated(List<Vector2> pointPath);
    public abstract SteeringGoal Suggest(List<Vector2> pointPath, SteeringGoal goal);
}
