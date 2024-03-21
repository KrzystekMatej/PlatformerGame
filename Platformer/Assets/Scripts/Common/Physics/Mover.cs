using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    public UnityEvent OnMovementFinished = new UnityEvent();

    private Vector2 start;
    private Rigidbody2D rigidBody;
    private float flyDistance;

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
        if (((Vector2)transform.position - start).magnitude >= flyDistance) OnMovementFinished?.Invoke();
    }
}
