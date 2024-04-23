using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class GotHit : ConditionNode
{
    [SerializeField]
    NodeProperty<Collider2D> attacker;
    [SerializeField]
    NodeProperty<Weapon> damageWeapon;

    bool hit = false;

    public override void OnInit()
    {
        context.Agent.OnHit.AddListener(SetHit);
    }

    public void SetHit(Collider2D attacker, Weapon damageWeapon)
    {
        hit = true;
        this.attacker.Value = attacker;
        this.damageWeapon.Value = damageWeapon;
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
