using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float angularVelocity;

    public void Initialize(float angularVelocity)
    {
        this.angularVelocity = angularVelocity;
    }

    private void Update()
    {
        Rotate();
    }
    
    private void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * angularVelocity);
    }
}
