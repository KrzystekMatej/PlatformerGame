using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Windows;

public class FallState : WalkState
{
    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new GenericToAttackTransition(),
            new FallToIdleTransition(),
            new FallToWalkTransition(),
            new GenericToClimbTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Fall);
    }

    public override void HandleUpdate()
    {
        ControlFall();
        CalculateVelocity();
    }

    private void ControlFall()
    {
        agent.InstanceData.Velocity = agent.RigidBody.velocity;
        agent.InstanceData.Velocity.y += agent.InstanceData.FallGravityModifier * Physics2D.gravity.y * Time.deltaTime;
        agent.RigidBody.velocity = agent.InstanceData.Velocity;
    }

    protected override void HandleExit()
    {
        agent.AudioFeedback.PlaySound(SoundActionType.Land, agent.GroundDetector.CollisionLayerIndex);
    }
}
