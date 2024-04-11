using UnityEngine;

public class IdleState : State
{
    protected override void HandleEnter() => agent.RigidBody.velocity = Vector2.zero;

    protected override void HandleUpdate() { }

    protected override void HandleExit() { }
}