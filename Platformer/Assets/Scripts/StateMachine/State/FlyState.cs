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

    protected override void CalculateVelocity()
    {
        agent.InstanceData.Velocity.x = CalculateVelocityComponent(agent.InputController.InputData.MovementVector.x, agent.InstanceData.Velocity.x);
        agent.InstanceData.Velocity.y = CalculateVelocityComponent(agent.InputController.InputData.MovementVector.y, agent.InstanceData.Velocity.y);

        agent.RigidBody.velocity = agent.InstanceData.Velocity;
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayFlapSound);
    }
}
