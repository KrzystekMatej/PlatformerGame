using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0;

    protected Vector2 start;
    protected Vector2 direction;
    protected Rigidbody2D rigidBody;
    protected float flyDistance;

    private void Awake()
    {
        start = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Rotate();
        if (((Vector2)transform.position - start).magnitude >= flyDistance)
        {
            Destroy(gameObject);
        }
    }

    public abstract void PerformHit(Collider2D collision);

    private void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationSpeed * -direction.x);
    }
}
