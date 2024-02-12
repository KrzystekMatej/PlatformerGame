using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

public class JumpState : WalkState
{
    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new GenericToAttackTransition(),
            new GenericToClimbTransition(),
            new JumpToFallTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Jump);
        agent.AudioFeedback.PlaySpecificSound(agent.GroundDetector.GetGroundSound(SoundActionType.Jump));
        agent.RigidBody.velocity = new Vector2(agent.RigidBody.velocity.x, agent.InstanceData.JumpForce);
    }

    public override void HandleUpdate()
    {
        ControlJumpHeight();
        CalculateAcceleration();
        CalculateVelocity();
    }

    private void ControlJumpHeight()
    {
        if (agent.InputController.InputData.Jump == InputState.Inactive)
        {
            agent.InstanceData.Acceleration.y += agent.InstanceData.JumpGravityModifier * Physics2D.gravity.y;
        }
    }
}
