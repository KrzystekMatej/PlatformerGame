using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParentSwitcher : MonoBehaviour
{
    public void Switch(Collider2D trigger)
    {
        ParentManager parentController = trigger.GetComponent<ParentManager>();
        if (parentController != null) parentController.SetTemporaryParent(transform);
    }

    public void Restore(Collider2D trigger)
    {
        ParentManager parentController = trigger.GetComponent<ParentManager>();
        if (parentController != null) parentController.RestorePrimaryParent();
    }
}