using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : WalkState
{
    [SerializeField]
    Sound flapSound;

    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new GenericToAttackTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Walk);
        agent.Animator.OnAnimationAction.AddListener(PlayFlapSound);
        agent.RigidBody.gravityScale = 0;
    }

    private void PlayFlapSound()
    {
        agent.AudioFeedback.PlaySpecificSound(flapSound);
    }

    protected override void CalculateAcceleration()
    {
        Vector2 input = agent.InputController.InputData.MovementVector;
        Vector2 velocityNormalized = agent.RigidBody.velocity.normalized;

        Vector2 direction = new Vector2(input.x == 0 ? -velocityNormalized.x : input.x, input.y == 0 ? -velocityNormalized.y : input.y);

        agent.InstanceData.Acceleration += direction * agent.InstanceData.MaxForce;
    }

    protected override void CalculateVelocity()
    {
        Vector2 previousVelocity = agent.RigidBody.velocity;

        agent.RigidBody.velocity += agent.InstanceData.Acceleration * Time.deltaTime;
        agent.RigidBody.velocity = new Vector2
        (
            StopDeceleration(agent.InputController.InputData.MovementVector.x, agent.RigidBody.velocity.x, previousVelocity.x),
            StopDeceleration(agent.InputController.InputData.MovementVector.y, agent.RigidBody.velocity.y, previousVelocity.y)
        );
        agent.RigidBody.velocity = Vector2.ClampMagnitude(agent.RigidBody.velocity, agent.InstanceData.MaxSpeed);

        agent.InstanceData.Acceleration.Set(0, 0);
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayFlapSound);
    }
}
