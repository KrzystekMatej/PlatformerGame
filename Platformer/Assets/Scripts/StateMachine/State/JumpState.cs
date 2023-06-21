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
        agent.AudioFeedback.PlaySound(SoundActionType.Jump, agent.GroundDetector.CollisionLayerIndex);
        agent.InstanceData.Velocity = agent.RigidBody.velocity;
        agent.InstanceData.Velocity.y = agent.InstanceData.JumpForce;
        agent.RigidBody.velocity = agent.InstanceData.Velocity;
    }

    public override void HandleUpdate()
    {
        ControlJumpHeight();
        CalculateVelocity();
    }

    private void ControlJumpHeight()
    {
        if (agent.InputController.InputData.Jump == InputState.Inactive)
        {
            agent.InstanceData.Velocity = agent.RigidBody.velocity;
            agent.InstanceData.Velocity.y += agent.InstanceData.JumpGravityModifier * Physics2D.gravity.y * Time.deltaTime;
            agent.RigidBody.velocity = agent.InstanceData.Velocity;
        }
    }
}
