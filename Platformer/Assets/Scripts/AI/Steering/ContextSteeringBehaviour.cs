using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextSteeringBehaviour : MonoBehaviour
{
    [SerializeField]
    protected OverlapDetector overlapDetector;

    public abstract void ModifySteeringContext(Agent agent, float[] danger, float[] interest, List<Vector2> directions);
}
