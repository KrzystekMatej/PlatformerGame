using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Windows;

public class OrientationController : MonoBehaviour
{
    [SerializeField]
    private bool flipOnStart;
    [SerializeField]
    private Collider2D objectCollider;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float flipThreshold = 1.1f;

    public Vector2 CurrentOrientation { get; private set; } = Vector2.right;


    private void Awake()
    {
        objectCollider = objectCollider ? objectCollider : GetComponent<Collider2D>();
        rigidBody = rigidBody ? rigidBody : GetComponentInParent<Rigidbody2D>();
        if (flipOnStart) Flip();
    }


    public void SetAgentOrientation()
    {
        const float minMagnitude = 0.001f;
        float magnitude = rigidBody.velocity.magnitude;
        if (minMagnitude < magnitude && Mathf.Abs(rigidBody.velocity.normalized.x - CurrentOrientation.x) > flipThreshold)
        {
            Flip();
        }
    }


    public void Flip()
    {
        CurrentOrientation = -CurrentOrientation;
        transform.parent.localScale = new Vector3
        (
            Mathf.Sign(CurrentOrientation.x) * Mathf.Abs(transform.parent.localScale.x),
            transform.parent.localScale.y,
            transform.parent.localScale.z
        );

        transform.parent.position = new Vector2(transform.parent.position.x - CurrentOrientation.x * objectCollider.offset.x * 2, transform.parent.position.y);
    }
}
