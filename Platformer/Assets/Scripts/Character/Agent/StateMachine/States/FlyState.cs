using UnityEngine;

public class FlyState : WalkState
{
    [SerializeField]
    Sound flapSound;

    protected override void HandleEnter()
    {
        agent.Animator.OnAnimationAction.AddListener(PlayFlapSound);
        agent.RigidBody.gravityScale = 0;
    }

    private void PlayFlapSound()
    {
        agent.AudioFeedback.PlaySpecificSound(flapSound);
    }

    protected override void CalculateAcceleration()
    {
        Vector2 inputForce = agent.InputController.InputData.SteeringForce;
        Vector2 decelerationForce = -MathUtility.GetSignedVector(agent.RigidBody.velocity) * agent.InstanceData.MaxForce;

        Vector2 force = new Vector2
        (
            agent.InputController.DecelerationFlags.x ? decelerationForce.x : inputForce.x,
            agent.InputController.DecelerationFlags.y ? decelerationForce.y : inputForce.y
        );


        agent.InstanceData.Acceleration += Vector2.ClampMagnitude(force, agent.InstanceData.MaxForce);
    }

    protected override void CalculateVelocity()
    {
        Vector2 previousVelocity = agent.RigidBody.velocity;

        agent.RigidBody.velocity += agent.InstanceData.Acceleration * Time.deltaTime;

        if (agent.InputController.DecelerationFlags.x && ShouldDecelerationStop(agent.RigidBody.velocity.x, previousVelocity.x))
        {
            agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
            agent.InputController.DecelerationFlags.x = false;
        }
        if (agent.InputController.DecelerationFlags.y && ShouldDecelerationStop(agent.RigidBody.velocity.y, previousVelocity.y))
        {
            agent.RigidBody.velocity = new Vector2(agent.RigidBody.velocity.x, 0);
            agent.InputController.DecelerationFlags.y = false;
        }

        agent.RigidBody.velocity = Vector2.ClampMagnitude(agent.RigidBody.velocity, agent.InstanceData.MaxSpeed);

        agent.InstanceData.Acceleration.Set(0, 0);
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayFlapSound);
    }
}