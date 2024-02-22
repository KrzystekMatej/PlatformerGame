using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour
{
    public abstract bool TryUpdateGoal(Agent agent, SteeringGoal goal);
}
