using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class ClimbState : State
{
    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new ClimbToIdleTransition(),
            new ClimbToJumpTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Climb);
        agent.Animator.Disable();
        agent.RigidBody.gravityScale = 0;
        agent.RigidBody.velocity = Vector3.zero;
    }

    public override void HandleUpdate()
    {
        if (agent.InputController.InputData.MovementVector.magnitude > 0)
        {
            agent.Animator.Enable();
            agent.RigidBody.velocity = agent.InputController.InputData.MovementVector * agent.InstanceData.ClimbSpeed;
        }
        else
        {
            agent.Animator.Disable();
            agent.RigidBody.velocity = Vector2.zero;
        }
    }

    protected override void HandleExit()
    {
        agent.RigidBody.gravityScale = agent.InstanceData.DefaultGravityScale;
        agent.Animator.Enable();
    }
}
