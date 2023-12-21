using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextSteeringBehaviour : MonoBehaviour
{
    protected AreaDetector areaDetector;

    public abstract void ModifySteeringContext(float[] danger, float[] interest, List<Vector2> directions);
}
