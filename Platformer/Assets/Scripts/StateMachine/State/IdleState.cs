using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class IdleState : State
{
    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new GenericToAttackTransition(),
            new GenericToJumpTransition(),
            new IdleToWalkTransition(),
            new GenericToClimbTransition(),
            new GenericToFallTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    public override void HandleUpdate()
    {
        agent.RigidBody.velocity = Vector2.zero;
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Idle);
        if (agent.GroundDetector.CollisionDetected)
        {
            agent.RigidBody.velocity = Vector2.zero;
        }
    }
}
