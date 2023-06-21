using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new HurtToIdleTransition(),
            new GenericToDieTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Hurt);
    }
}
