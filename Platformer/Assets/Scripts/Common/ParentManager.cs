using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [field: SerializeField]
    public bool isStickable { get; set; }


    private Transform primaryParent;
    public Transform PrimaryParent 
    {
        get => primaryParent;
        set
        {
            primaryParent = value;
            transform.SetParent(value);
        }
    }

    private void Awake()
    {
        primaryParent = transform.parent;
    }

    public void RestorePrimaryParent()
    {
        transform.SetParent(primaryParent);
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
