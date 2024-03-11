using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentAnimator : MonoBehaviour
{
    private Animator animator;

    public UnityEvent OnAnimationAction;
    public UnityEvent OnAnimationComplete;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayByType(AnimationType animationType)
    {
        animator.Play(animationType.ToString(), -1, 0f);
    }
    public void Enable()
    {
        animator.enabled = true;
    }

    public void Disable()
    {
        animator.enabled = false;
    }

    public void ResetEvents()
    {
        OnAnimationAction.RemoveAllListeners();
        OnAnimationComplete.RemoveAllListeners();
    }

    public void InvokeAnimationAction()
    {
        OnAnimationAction?.Invoke();
    }

    public void InvokeAnimationComplete()
    {
        OnAnimationComplete?.Invoke();
    }
}


public enum AnimationType
{
    Die,
    Hurt,
    Idle,
    Attack,
    Walk,
    Jump,
    Fall,
    Climb,
    Land,
    Fly
}
