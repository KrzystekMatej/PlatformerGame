using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Targeter
{
    SteeringGoal GetGoal(Agent agent);
}
