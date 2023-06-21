using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask triggerMask;
    [field: SerializeField]
    public bool Triggered { get; private set; }
    public UnityEvent<Collider2D> OnEnter, OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Triggered = Utility.CheckLayer(collision.gameObject.layer, triggerMask);
        if (Triggered) OnEnter?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        Triggered = Triggered && !Utility.CheckLayer(collision.gameObject.layer, triggerMask);
        if (!Triggered) OnExit?.Invoke(collision);
    }

    public void Enable()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    public void Disable()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
