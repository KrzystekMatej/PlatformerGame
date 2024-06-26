using UnityEngine;

public class JumpState : WalkState
{
    protected override void HandleEnter()
    {
        agent.AudioFeedback.PlaySpecificSound(agent.GroundDetector.GetGroundSound(StateType.Jump));
        agent.RigidBody.velocity = new Vector2(agent.RigidBody.velocity.x, agent.InstanceData.JumpForce);
    }

    protected override void HandleUpdate()
    {
        ControlJumpHeight();
        base.HandleUpdate();
    }

    private void ControlJumpHeight()
    {
        if (agent.InputController.InputData.Jump == InputState.Inactive)
        {
            agent.InstanceData.Acceleration.y += agent.DefaultData.JumpGravityModifier * Physics2D.gravity.y;
        }
    }

    protected override void HandleExit() { }
}
