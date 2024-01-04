using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class GotHit : Condition
{
    bool hit = false;

    public override void Initialize()
    {
        context.Agent.OnHit.AddListener(SetHit);
    }

    public void SetHit(Collider2D attackerCollider, Weapon attackingWeapon)
    {
        hit = true;
        blackboard.DataTable["OpponentPosition"] = attackerCollider.bounds.center;
        blackboard.DataTable["AttackingWeapon"] = attackingWeapon;
    }

    protected override bool IsConditionSatisfied()
    {
        bool temp = hit;
        hit = false;
        return temp;
    }
}
