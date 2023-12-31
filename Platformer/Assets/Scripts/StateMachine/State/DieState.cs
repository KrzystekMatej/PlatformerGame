using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    [SerializeField]
    private float deathDuration = 2f;

    protected override StateTransition[] GetTransitions()
    {
        return Array.Empty<StateTransition>();
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Die);
        agent.Animator.OnAnimationComplete.AddListener(CompleteTheDeath);
        agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
    }

    private void CompleteTheDeath()
    {
        agent.Animator.OnAnimationComplete.RemoveListener(CompleteTheDeath);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(deathDuration);
        agent.OnDeathComplete?.Invoke();
    }

    public override void HandleUpdate()
    {
        agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
    }
}
