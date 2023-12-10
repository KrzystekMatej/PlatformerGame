using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    protected Vector2 start;
    protected Vector2 direction;
    protected Rigidbody2D rigidBody;
    protected float flyDistance;

    private void Awake()
    {
        start = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float flyDistance, Vector2 direction, float flySpeed)
    {
        this.flyDistance = flyDistance;
        this.direction = direction;
        this.rigidBody.velocity = direction * flySpeed;
    }


    private void Update()
    {
        if (((Vector2)transform.position - start).magnitude >= flyDistance)
        {
            Destroy(gameObject);
        }
    }
}
