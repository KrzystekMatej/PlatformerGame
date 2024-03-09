using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected Sound collectSound;

    public abstract void Collect(Collider2D collider);

}
