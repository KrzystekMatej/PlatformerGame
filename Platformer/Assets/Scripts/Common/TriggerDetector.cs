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

    public bool IsColliderTriggered(Collider2D trigger)
    {
        return triggerSet.Contains(trigger);
    }

    private bool CheckTrigger(Collider2D trigger)
    {
        return Utility.CheckLayer(trigger.gameObject.layer, triggerMask) && (triggerTag == "" || trigger.CompareTag(triggerTag));
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (CheckTrigger(trigger))
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


