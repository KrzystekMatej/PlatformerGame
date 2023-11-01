using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 0;

    protected Vector2 direction;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationSpeed * -direction.x);
    }
}
