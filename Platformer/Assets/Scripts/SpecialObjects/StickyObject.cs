using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask triggerMask;
    private Dictionary<GameObject, GameObject> parentTable = new Dictionary<GameObject, GameObject>();


    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (Utility.CheckLayer(trigger.gameObject.layer, triggerMask))
        {
            parentTable[trigger.gameObject] = trigger.gameObject.transform.parent.gameObject;
            trigger.gameObject.transform.SetParent(transform);
        }
    }

    public void OnTriggerExit2D(Collider2D trigger)
    {
        if (Utility.CheckLayer(trigger.gameObject.layer, triggerMask))
        {
            trigger.gameObject.transform.SetParent(parentTable[trigger.gameObject].transform);
            parentTable.Remove(trigger.gameObject);
        }
    }
}
