using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerFilter : MonoBehaviour
{
    [SerializeField]
    private Collider2D triggerCollider;
    [SerializeField]
    private LayerMask triggerMask;
    [field: SerializeField]
    public int TriggerCounter { get; private set; }
    public UnityEvent<Collider2D> OnEnter, OnExit;

    private HashSet<Collider2D> triggerSet = new HashSet<Collider2D>();

    private void Awake()
    {
        triggerCollider = triggerCollider ? triggerCollider : GetComponent<Collider2D>();
    }

    public void Enable()
    {
        triggerCollider.enabled = true;
    }

    public void Disable()
    {
        triggerCollider.enabled = false;
    }

    public void ChangeTriggerMask(LayerMask triggerMask)
    {
        this.triggerMask = triggerMask;
    }

    public bool IsColliderTriggered(Collider2D trigger)
    {
        return triggerSet.Contains(trigger);
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (Utility.CheckLayer(trigger.gameObject.layer, triggerMask))
        {
            triggerSet.Add(trigger);
            TriggerCounter++;
            OnEnter?.Invoke(trigger);
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (triggerSet.Remove(trigger))
        {
            TriggerCounter--;
            OnExit?.Invoke(trigger);
        }
    }
}


