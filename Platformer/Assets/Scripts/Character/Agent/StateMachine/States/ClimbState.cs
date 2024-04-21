using UnityEngine;

public class ClimbState : State
{

    protected override void HandleEnter()
    {
        agent.RigidBody.gravityScale = 0;
        agent.RigidBody.velocity = Vector2.zero;
    }

    protected override void HandleUpdate()
    {
        Vector2 steeringForce = agent.InputController.InputData.SteeringForce;
        if (steeringForce.magnitude > 0)
        {
            agent.Animator.Enable();
            agent.RigidBody.velocity = MathUtility.GetSignedVector(steeringForce) * agent.InstanceData.ClimbSpeed;
        }
        else
        {
            agent.Animator.Disable();
            agent.RigidBody.velocity = Vector2.zero;
        }
    }

    protected override void HandleExit()
    {
        agent.RigidBody.gravityScale = agent.DefaultData.GravityScale;
        agent.Animator.Enable();
    }
}