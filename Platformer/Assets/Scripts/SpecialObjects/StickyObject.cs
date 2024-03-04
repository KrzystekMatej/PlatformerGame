using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StickyObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        ParentController parentController = trigger.GetComponent<ParentController>();
        if (parentController != null) parentController.SetTemporaryParent(transform);
    }

    public void OnTriggerExit2D(Collider2D trigger)
    {
        ParentController parentController = trigger.GetComponent<ParentController>();
        if (parentController != null) parentController.RestoreControlParent();
    }
}