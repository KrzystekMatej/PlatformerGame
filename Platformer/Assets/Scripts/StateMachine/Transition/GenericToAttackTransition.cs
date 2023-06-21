using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericToAttackTransition : StateTransition
{
    public GenericToAttackTransition() : base(StateType.Attack) { }

    public override bool IsTriggered(Agent agent)
    {
        Weapon weapon = agent.WeaponManager.GetWeapon();
        return agent.InputController.InputData.Attack == InputState.Pressed && weapon != null && weapon.IsUseable(agent);
    }
}
