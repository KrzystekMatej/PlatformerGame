using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;

    public UnityEvent OnAnimationComplete;
    public UnityEvent OnAnimationAction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayByType(StateType animationType)
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
