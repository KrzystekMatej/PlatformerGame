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
        if (agent.GroundDetector.Hit) agent.AudioFeedback.PlaySound(SoundActionType.Step, agent.GroundDetector.Hit.collider.gameObject.layer);
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

    protected virtual void CalculateVelocity()
    {
        Vector2 previousVelocity = agent.RigidBody.velocity;

        agent.RigidBody.velocity += agent.InstanceData.Acceleration * Time.deltaTime;
        agent.RigidBody.velocity = new Vector2
        (
            StopDeceleration(agent.InputController.InputData.MovementVector.x, agent.RigidBody.velocity.x, previousVelocity.x),
            agent.RigidBody.velocity.y
        );
        agent.RigidBody.velocity = new Vector2
        (
            Mathf.Clamp(agent.RigidBody.velocity.x, -agent.InstanceData.MaxSpeed, agent.InstanceData.MaxSpeed),
            agent.RigidBody.velocity.y
        );

        agent.InstanceData.Acceleration.Set(0, 0);
    }

    protected static float StopDeceleration(float input, float currentVelocity, float previousVelocity)
    {
        return input == 0 && Mathf.Sign(currentVelocity) != Mathf.Sign(previousVelocity) ? 0 : currentVelocity;
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayStepSound);
    }
}
