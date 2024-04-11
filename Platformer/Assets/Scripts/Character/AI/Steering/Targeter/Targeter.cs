using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public abstract class Targeter : PipelineComponent
{
    public abstract ProcessState TryUpdateGoal(SteeringGoal goal);
}
