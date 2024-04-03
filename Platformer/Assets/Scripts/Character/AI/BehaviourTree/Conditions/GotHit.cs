using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class GotHit : ConditionNode
{
    bool hit = false;

    public override void OnInit()
    {
        context.Agent.OnHit.AddListener(SetHit);
    }

    public void SetHit(Collider2D attackerCollider, Weapon attackingWeapon)
    {
        hit = true;
        blackboard.SetValue("AttackerPosition", (Vector2)attackerCollider.bounds.center);
        blackboard.SetValue("AttackingWeapon", attackingWeapon);
    }

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        bool temp = hit;
        hit = false;
        return temp;
    }

    protected override void OnStop() { }
}
