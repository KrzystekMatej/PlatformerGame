using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanAttackAgent : Condition
{
    private AttackState attackState;

    public override void Initialize()
    {
        attackState = context.Agent.GetComponentInChildren<AttackState>();
    }

    protected override bool IsConditionSatisfied()
    {
        AgentWeapon weapon = context.Agent.WeaponManager.GetWeapon();
        int detectionCount = 0;
        if (weapon != null && weapon.IsUseable(context.Agent))
        {
            Vector2 attackDirection = context.Agent.transform.right * context.Agent.OrientationController.CurrentOrientation;
            Vector2 origin = context.Agent.CenterPosition + attackDirection * weapon.AttackDetector.Size.x/2;
            weapon.AttackDetector.DetectLayerMask = attackState.HitMask;
            detectionCount = weapon.AttackDetector.Detect(origin);
        }
        return detectionCount != 0;
    }
}
