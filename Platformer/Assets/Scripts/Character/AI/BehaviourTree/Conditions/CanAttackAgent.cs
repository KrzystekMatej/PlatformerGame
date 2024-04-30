using UnityEngine;

[System.Serializable]
public class CanAttackAgent : ConditionNode
{
    protected override bool IsConditionSatisfied()
    {
        AttackState attackState = (AttackState)context.Agent.StateMachine.Factory.GetState(StateType.Attack);
        if (!attackState) return false;

        AttackingWeapon weapon = context.Agent.WeaponManager.GetWeapon();
        int targetCount = 0;
        if (weapon && weapon.IsUseable(context.Agent))
        {
            targetCount = weapon.DetectInAttackRange(context.Agent.TriggerCenter, context.Agent.OrientationController.CurrentOrientation, attackState.HitMask);
        }
        return targetCount != 0;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos(AgentManager agent)
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        AttackingWeapon weapon = agent.GetComponentInChildren<WeaponManager>().GetWeapon();
        if (weapon) weapon.DrawGizmos(agent.TriggerCenter, context.Agent.OrientationController.CurrentOrientation);
    }
#endif
}
