using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParentSwitcher : MonoBehaviour
{
    public void Switch(Collider2D trigger)
    {
        ParentManager parentManager = trigger.GetComponent<ParentManager>();
        if (parentManager)
        {
            parentManager.SetTemporaryParent(transform);
        }
    }

    public void Restore(Collider2D trigger)
    {
        ParentManager parentManager = trigger.GetComponent<ParentManager>();
        if (parentManager)
        {
            parentManager.RestorePrimaryParent();
        }
    }
}