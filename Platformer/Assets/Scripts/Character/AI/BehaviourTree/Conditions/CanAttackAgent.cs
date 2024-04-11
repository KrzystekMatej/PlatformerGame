using UnityEngine;

[System.Serializable]
public class CanAttackAgent : ConditionNode
{
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
            targetCount = weapon.DetectInAttackRange(context.Agent.CenterPosition, context.Agent.OrientationController.CurrentOrientation, attackState.HitMask);
        }
        return targetCount != 0;
    }

    protected override void OnStop() { }

#if UNITY_EDITOR
    public override void OnDrawGizmos(AgentManager agent)
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        AttackingWeapon weapon = agent.GetComponentInChildren<WeaponManager>().GetWeapon();
        if (weapon != null)
        {
            weapon.DrawGizmos(agent.CenterPosition, context.Agent.OrientationController.CurrentOrientation);
        }
    }
#endif
}
