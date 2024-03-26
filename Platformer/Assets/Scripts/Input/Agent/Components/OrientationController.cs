using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Windows;

public class OrientationController : MonoBehaviour
{
    [SerializeField]
    private Collider2D objectCollider;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float flipThreshold = 1.1f;

    public float CurrentOrientation { get; private set; } = 1f;


    private void Awake()
    {
        objectCollider = objectCollider ? objectCollider : GetComponent<Collider2D>();
        rigidBody = rigidBody ? rigidBody : GetComponentInParent<Rigidbody2D>();
    }


    public void SetAgentOrientation()
    {
        const float minMagnitude = 0.001f;
        float magnitude = rigidBody.velocity.magnitude;
        if (minMagnitude < magnitude && Mathf.Abs(rigidBody.velocity.normalized.x - CurrentOrientation) > flipThreshold)
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
