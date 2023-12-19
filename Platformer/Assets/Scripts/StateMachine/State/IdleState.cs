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

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Idle);
        agent.RigidBody.velocity = agent.GroundDetector.CollisionDetected ? Vector2.zero : agent.RigidBody.velocity;
    }
}
