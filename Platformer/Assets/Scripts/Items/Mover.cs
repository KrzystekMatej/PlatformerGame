using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    protected Vector2 start;
    protected Rigidbody2D rigidBody;
    protected float flyDistance;

    private void Awake()
    {
        start = transform.position;
        rigidBody = gameObject.AddComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0;
    }

    public void Initialize(float flyDistance, Vector2 velocity)
    {
        this.flyDistance = flyDistance;
        rigidBody.velocity = velocity;
    }


    private void Update()
    {
        if (((Vector2)transform.position - start).magnitude >= flyDistance)
        {
            Destroy(gameObject);
        }
    }
}
