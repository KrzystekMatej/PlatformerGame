using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public abstract class BehaviourTreeInitializer : MonoBehaviour
{
    public abstract void Initialize(BehaviourTree behaviourTree, Context context);
}
