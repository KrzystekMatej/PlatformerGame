using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour
{
    public abstract SteeringGoal GetGoal(Agent agent);
}
