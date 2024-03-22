using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericToAttackTransition : StateTransition
{
    public GenericToAttackTransition() : base(StateType.Attack) { }

    public override bool IsTriggered(AgentManager agent)
    {
        AttackingWeapon weapon = agent.WeaponManager.GetWeapon();
        return agent.InputController.InputData.Attack == InputState.Pressed && weapon != null && weapon.IsUseable(agent);
    }
}
