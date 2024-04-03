using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actuator : PipelineComponent
{
    public abstract List<Vector2> GetPath(SteeringGoal goal);
    public abstract Vector2? GetSteering(List<Vector2> pointPath, SteeringGoal goal);
}
