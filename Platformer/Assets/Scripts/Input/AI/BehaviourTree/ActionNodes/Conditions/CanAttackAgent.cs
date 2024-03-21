using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Unity.VisualScripting;
using static UnityEngine.UI.Image;

[System.Serializable]
public class CanAttackAgent : Condition
{
    [SerializeField]
    private bool useStopApproximation = true;

    private AttackState attackState;

    public override void OnInit()
    {
        attackState = context.Agent.GetComponentInChildren<AttackState>();
    }

    protected override void OnStart() { }

    protected override bool IsConditionSatisfied()
    {
        AttackingWeapon weapon = context.Agent.WeaponManager.GetWeapon();
        int targetCount = 0;
        if (weapon != null && weapon.IsUseableByAgent(context.Agent))
        {
            float orientation = context.Agent.OrientationController.CurrentOrientation;
            Vector2 offset = useStopApproximation
                ? MathUtility.CalculateStopOffset(context.Agent.RigidBody.velocity, context.Agent.InstanceData.MaxForce) * orientation
                : Vector2.zero;
            targetCount = weapon.DetectInAttackRange(context.Agent.TriggerCollider, attackState.HitMask, context.Agent.transform.right * orientation, offset);
        }
        return targetCount != 0;
    }

    protected override void OnStop() { }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        AttackingWeapon weapon = context.Agent.WeaponManager.GetWeapon();
        if (weapon != null && weapon.IsUseableByAgent(context.Agent))
        {
            float orientation = context.Agent.OrientationController.CurrentOrientation;
            Vector2 offset = Vector2.zero;
            if (useStopApproximation) offset = MathUtility.CalculateStopOffset(context.Agent.RigidBody.velocity, context.Agent.InstanceData.MaxForce);
            weapon.DrawGizmos(context.Agent.CenterPosition, context.Agent.transform.right * context.Agent.OrientationController.CurrentOrientation, offset * orientation);
        }
    }
}
