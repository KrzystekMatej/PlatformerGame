using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    private string currentStateName;

    public UnityEvent OnAnimationComplete;
    public UnityEvent OnAnimationAction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.Play(currentStateName, -1, 0f);
    }

    public void PlayByType(StateType state)
    {
        currentStateName = state.ToString();
        animator.Play(currentStateName, -1, 0f);
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
