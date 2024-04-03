using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decomposer : PipelineComponent
{
    public abstract SteeringGoal Decompose(SteeringGoal goal);
}
