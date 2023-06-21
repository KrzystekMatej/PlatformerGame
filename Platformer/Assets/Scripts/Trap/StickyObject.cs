using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObject : MonoBehaviour
{
    private Dictionary<GameObject, GameObject> parentTable = new Dictionary<GameObject, GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject passenger = Utility.GetParentIf(collision.gameObject, (obj) => obj.CompareTag("Player"));
        if (passenger != null)
        {
            parentTable[passenger] = passenger.transform.parent.gameObject;
            passenger.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        GameObject passenger = Utility.GetParentIf(collision.gameObject, (obj) => obj.CompareTag("Player"));
        if (passenger != null)
        {
            passenger.transform.SetParent(parentTable[passenger].transform);
            parentTable.Remove(passenger);
        }
    }
}
