using UnityEngine;

public class FallState : WalkState
{
    protected override void HandleEnter()
    {
        agent.RigidBody.gravityScale = agent.DefaultData.GravityScale;
    }

    protected override void HandleUpdate()
    {
        ControlFall();
        base.HandleUpdate();
    }

    private void ControlFall()
    {
        agent.InstanceData.Acceleration.y += agent.DefaultData.FallGravityModifier * Physics2D.gravity.y;
    }

    protected override void HandleExit()
    {
        agent.AudioFeedback.PlaySpecificSound(agent.GroundDetector.GetGroundSound(StateType.Fall));
    }
}