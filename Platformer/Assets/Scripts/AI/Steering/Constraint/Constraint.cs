using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Constraint
{
    bool IsViolated(Path path);
    SteeringGoal Suggest(Agent agent, Path path, SteeringGoal goal);
}
