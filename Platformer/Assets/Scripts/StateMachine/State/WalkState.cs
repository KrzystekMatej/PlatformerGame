using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

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
        CalculateAcceleration();
        CalculateVelocity();
    }

    

    protected virtual void CalculateAcceleration()
    {
        Vector2 input = agent.InputController.InputData.MovementVector;
        Vector2 velocityNormalized = agent.RigidBody.velocity.normalized;

        Vector2 direction = new Vector2(input.x == 0 ? -velocityNormalized.x : input.x, 0);

        agent.InstanceData.Acceleration += direction * agent.InstanceData.MaxForce;
    }

    protected void CalculateVelocity()
    {
        Vector2 previousVelocity = agent.RigidBody.velocity;

        agent.RigidBody.velocity += agent.InstanceData.Acceleration * Time.deltaTime;

        agent.RigidBody.velocity = new Vector2
        (
            StopDeacceleration(agent.InputController.InputData.MovementVector.x, agent.RigidBody.velocity.x, previousVelocity.x),
            StopDeacceleration(agent.InputController.InputData.MovementVector.y, agent.RigidBody.velocity.y, previousVelocity.y)
        );

        LimitVelocity();
        agent.InstanceData.Acceleration.Set(0, 0);
    }

    private static float StopDeacceleration(float input, float currentVelocity, float previousVelocity)
    {
        if (input == 0 && Mathf.Sign(currentVelocity) != Mathf.Sign(previousVelocity))
        {
            return 0;
        }
        return currentVelocity;
    }

    protected virtual void LimitVelocity()
    {
        agent.RigidBody.velocity = new Vector2
        (
            Mathf.Clamp(agent.RigidBody.velocity.x, -agent.InstanceData.MaxSpeed, agent.InstanceData.MaxSpeed),
            agent.RigidBody.velocity.y
        );
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayStepSound);
    }
}
