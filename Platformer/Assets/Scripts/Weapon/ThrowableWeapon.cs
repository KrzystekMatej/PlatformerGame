using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 1;

    private RangeWeapon rangeWeapon;
    private Vector2 start;
    private Vector2 direction;
    private Rigidbody2D rigidBody;
    private TriggerDetector triggerDetector;
    private Agent agent;

    private void Awake()
    {
        start = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        triggerDetector = GetComponent<TriggerDetector>();
    }

    public void Initialize(RangeWeapon rangeWeapon, Vector2 direction, LayerMask layerMask, Agent agent)
    {
        this.rangeWeapon = rangeWeapon;
        this.direction = direction;
        this.rigidBody.velocity = direction * rangeWeapon.FlySpeed;
        this.agent = agent;
    }

    private void Update()
    {
        Fly();
        if (((Vector2)transform.position - start).magnitude >= rangeWeapon.AttackRange)
        {
            Destroy(gameObject);
        }
    }

    public void PerformHit(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject == agent.gameObject) return;
            IHittable hittable = collision.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.Hit(agent.gameObject, rangeWeapon);
                if (!rangeWeapon.IsUnstoppable) Destroy(gameObject);
            }
        }
    }

    private void Fly()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationSpeed * -direction.x);
    }
}
