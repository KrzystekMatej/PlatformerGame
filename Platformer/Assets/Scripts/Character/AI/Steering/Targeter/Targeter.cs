using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : PipelineComponent
{
    public abstract bool TryUpdateGoal(SteeringGoal goal);
}
