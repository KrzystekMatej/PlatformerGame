using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask triggerMask;
    [SerializeField]
    private string triggerTag;
    [field: SerializeField]
    public int TriggerCounter { get; private set; }
    public UnityEvent<Collider2D> OnEnter, OnExit;

    private HashSet<Collider2D> triggerSet = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (CheckTrigger(trigger))
        {
            TriggerCounter++;
            triggerSet.Add(trigger);
            OnEnter?.Invoke(trigger);
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (CheckTrigger(trigger))
        {
            TriggerCounter--;
            triggerSet.Remove(trigger);
            OnExit?.Invoke(trigger);
        }
    }

    private bool CheckTrigger(Collider2D trigger)
    {
        return Utility.CheckLayer(trigger.gameObject.layer, triggerMask) && (triggerTag == "" || trigger.CompareTag(triggerTag));
    }

    public bool IsColliderTriggered(Collider2D trigger)
    {
        return triggerSet.Contains(trigger);
    }

    public void Enable()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void Disable()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public void ChangeTriggerMask(LayerMask triggerMask)
    {
        this.triggerMask = triggerMask;
    }
}


