using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    [field: SerializeField]
    public bool isStickable { get; set; }


    private Transform controlParent;
    public Transform ControlParent 
    {
        get => controlParent;
        set
        {
            controlParent = value;
            transform.SetParent(value);
        }
    }

    private void Awake()
    {
        controlParent = transform.parent;
    }

    public void RestoreControlParent()
    {
        transform.SetParent(controlParent);
    }

    public bool SetTemporaryParent(Transform temporaryParent)
    {
        if (isStickable)
        {
            transform.SetParent(temporaryParent);
            return true;
        }
        return false;
    }
}
