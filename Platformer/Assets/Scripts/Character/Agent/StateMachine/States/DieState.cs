using System.Collections;
using UnityEngine;

public class DieState : State
{
    [SerializeField]
    private float deathDuration = 2f;

    protected override void HandleEnter()
    {
        agent.RigidBody.gravityScale = agent.DefaultData.GravityScale;
        agent.Animator.OnAnimationComplete.AddListener(CompleteTheDeath);
    }

    private void CompleteTheDeath()
    {
        agent.Animator.OnAnimationComplete.RemoveListener(CompleteTheDeath);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(deathDuration);
        PerformExitActions();
    }

    protected override void HandleUpdate()
    {
        agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
    }

    protected override void HandleExit() { }
}