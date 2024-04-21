using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

[System.Serializable]
public class IsAlive : ConditionNode
{
    [SerializeField]
    NodeProperty<Collider2D> collider;

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        HealthManager healthManager = collider.Value.GetComponent<HealthManager>();
        return healthManager && healthManager.IsAlive();
    }

    protected override void OnStop() { }
}

