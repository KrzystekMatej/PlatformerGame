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
        context.agent.OnHit.AddListener(SetHit);
    }

    public void SetHit(GameObject gameObject, Weapon attackingWeapon)
    {
        hit = true;
        blackboard.DataTable["opponent"] = gameObject;
        blackboard.DataTable["attackingWeapon"] = attackingWeapon;
    }

    protected override bool IsConditionSatisfied()
    {
        bool temp = hit;
        hit = false;
        return temp;
    }
}
