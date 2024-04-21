using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decomposer : PipelineComponent
{
    public abstract bool Decompose(SteeringGoal goal);
}
