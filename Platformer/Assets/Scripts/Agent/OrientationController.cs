using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;

public class OrientationController : MonoBehaviour
{
    [SerializeField]
    private Collider2D objectCollider;
    

    public float CurrentOrientation { get; private set; } = 1f;


    private void Awake()
    {
        objectCollider = objectCollider == null ? GetComponent<Collider2D>() : objectCollider;
    }


    public void SetAgentOrientation(Vector2 input)
    {

        if (input.x != 0 && Mathf.Sign(input.x) != Mathf.Sign(CurrentOrientation))
        {
            Flip();
        }
    }


    public void Flip()
    {
        CurrentOrientation = -CurrentOrientation;
        transform.parent.localScale = new Vector3
        (
            Mathf.Sign(CurrentOrientation) * Mathf.Abs(transform.parent.localScale.x),
            transform.parent.localScale.y,
            transform.parent.localScale.z
        );

        transform.parent.position = new Vector2(transform.parent.position.x - CurrentOrientation * objectCollider.offset.x * 2, transform.parent.position.y);
    }
}
