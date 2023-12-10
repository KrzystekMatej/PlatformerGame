using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanAttackAgent : Condition
{
    protected override bool IsConditionSatisfied()
    {
        float attackDirection = context.Agent.OrientationController.CurrentOrientation;
        AgentWeapon weapon = context.Agent.WeaponManager.GetWeapon();
        if (weapon != null && weapon.IsUseable(context.Agent))
        {
            CastDetector detector = (CastDetector)context.AIManager.Vision.GetDetector(attackDirection == 1 ? "Right" + weapon.WeaponName : "Left" + weapon.WeaponName);
            RaycastHit2D hit = detector.Hit;
            if (hit.collider == null) return false;
            IHittable target = hit.collider.GetComponent<IHittable>();
            return target != null;
        }
        return false;
    }
}
