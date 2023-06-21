using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanAttackAgent : Condition
{
    [SerializeField]
    [Range(0f, 1f)]
    private float startAttackRangeModifier = 1f;

    protected override bool IsConditionSatisfied()
    {
        float attackDirection = context.agent.OrientationController.CurrentOrientation;
        RaycastHit2D hit = context.vision.GetRaycastHit(attackDirection == 1 ? "Right" : "Left");
        if (hit.collider == null) return false;
        Agent target = hit.collider.GetComponent<Agent>();
        return target != null
            && context.agent.WeaponManager.GetWeapon() != null
            && context.agent.WeaponManager.GetWeapon().IsUseable(context.agent)
            && hit.distance < context.agent.WeaponManager.GetWeapon().AttackRange * startAttackRangeModifier;
    }
}
