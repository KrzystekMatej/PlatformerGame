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
        if (weapon != null && weapon.IsUseable(context.Agent))
        {
            float orientation = context.Agent.OrientationController.CurrentOrientation;
            Vector2 offset = useStopApproximation
                ? MathUtility.CalculateStopOffset(context.Agent.RigidBody.velocity, context.Agent.InstanceData.MaxForce) * orientation
                : Vector2.zero;
            targetCount = weapon.DetectInAttackRange(context.Agent.CenterPosition, context.Agent.transform.right * orientation, attackState.HitMask, offset);
        }
        return targetCount != 0;
    }

    protected override void OnStop() { }

    public override void OnDrawGizmos(AgentManager agent)
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        AttackingWeapon weapon = agent.GetComponentInChildren<WeaponManager>().GetWeapon();
        if (weapon != null)
        {
            float orientation = agent.OrientationController.CurrentOrientation;
            Vector2 offset = Vector2.zero;
            if (useStopApproximation) offset = MathUtility.CalculateStopOffset(agent.RigidBody.velocity, agent.InstanceData.MaxForce);
            weapon.DrawGizmos(agent.CenterPosition, agent.transform.right * agent.OrientationController.CurrentOrientation, offset * orientation);
        }
    }
}
