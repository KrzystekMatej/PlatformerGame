using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected TriggerDetector triggerDetector;
    [SerializeField]
    protected Sound collectSound;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerDetector = GetComponent<TriggerDetector>();
    }

    public abstract void Collect(Collider2D collider);

}
