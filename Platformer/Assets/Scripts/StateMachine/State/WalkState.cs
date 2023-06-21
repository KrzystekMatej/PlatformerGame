using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WalkState : State
{
    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new GenericToAttackTransition(),
            new GenericToJumpTransition(),
            new WalkToIdleTransition(),
            new GenericToFallTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Walk);
        agent.Animator.OnAnimationAction.AddListener(PlayStepSound);
    }

    private void PlayStepSound()
    {
        agent.AudioFeedback.PlaySound(SoundActionType.Step, agent.GroundDetector.CollisionLayerIndex);
    }

    public override void HandleUpdate()
    {
        CalculateVelocity();
    }

    protected virtual void CalculateVelocity()
    {
        agent.InstanceData.Velocity.x = CalculateVelocityComponent(agent.InputController.InputData.MovementVector.x, agent.InstanceData.Velocity.x);
        agent.InstanceData.Velocity.y = agent.RigidBody.velocity.y;

        agent.RigidBody.velocity = agent.InstanceData.Velocity;
    }

    protected float CalculateVelocityComponent(float input, float currentVelocity)
    {
        if (input != 0)
        {
            return Accelerate(input, currentVelocity);
        }
        else
        {
            return Deaccelerate(currentVelocity);
        }
    }

    private float Accelerate(float input, float currentVelocity)
    {
        if (Mathf.Sign(currentVelocity) != Mathf.Sign(input))
        {
            currentVelocity = -currentVelocity;
        }

        currentVelocity += Mathf.Sign(input) * agent.InstanceData.Acceleration * Time.deltaTime;
        float modifiedMaxSpeed = agent.InstanceData.MaxSpeed * Mathf.Abs(input);
        return Mathf.Clamp(currentVelocity, -modifiedMaxSpeed, modifiedMaxSpeed);
    }

    private float Deaccelerate(float currentVelocity)
    {
        currentVelocity -= Mathf.Sign(currentVelocity) * agent.InstanceData.Deacceleration * Time.deltaTime;

        if (Mathf.Sign(currentVelocity) != Mathf.Sign(agent.InstanceData.Velocity.x))
        {
            currentVelocity = 0;
        }

        return currentVelocity;
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayStepSound);
    }
}
