using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteeringBehaviour
{
    Vector2 GetSteering(Agent agent, Vision vision);
}

