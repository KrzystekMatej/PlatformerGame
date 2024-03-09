using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 0;
    [SerializeField]
    public float rotateDirection = 1;

    public void Initialize(float rotateSpeed, float rotateDirection)
    {
        this.rotateSpeed = rotateSpeed;
        this.rotateDirection = rotateDirection;
    }

    private void Update()
    {
        Rotate();
    }
    
    private void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotateSpeed * rotateDirection);
    }
}
