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

    public void SetHit(Collider2D attacker, Weapon damageWeapon)
    {
        hit = true;
        blackboard.SetValue("Attacker", attacker);
        blackboard.SetValue("DamageWeapon", damageWeapon);
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
