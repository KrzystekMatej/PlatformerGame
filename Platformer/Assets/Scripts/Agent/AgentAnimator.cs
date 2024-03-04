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
        switch (animationType)
        {
            case AnimationType.Die:
                PlayByName("Die");
                break;
            case AnimationType.Hurt:
                PlayByName("Hurt");
                break;
            case AnimationType.Idle:
                PlayByName("Idle");
                break;
            case AnimationType.Attack:
                PlayByName("Attack");
                break;
            case AnimationType.Walk:
                PlayByName("Walk");
                break;
            case AnimationType.Jump:
                PlayByName("Jump");
                break;
            case AnimationType.Fall:
                PlayByName("Fall");
                break;
            case AnimationType.Climb:
                PlayByName("Climb");
                break;
            case AnimationType.Land:
                PlayByName("Land");
                break;
            default:
                break;
        }
    }
    public void Enable()
    {
        animator.enabled = true;
    }

    public void Disable()
    {
        animator.enabled = false;
    }

    public void PlayByName(string name)
    {
        animator.Play(name, -1, 0f);
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
    Land
}
