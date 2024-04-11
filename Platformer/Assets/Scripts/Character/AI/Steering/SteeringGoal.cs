using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringGoal
{
    public bool HasPosition { get; private set; }
    public bool HasSpeed { get; private set; }
    public bool HasOwner { get; private set; }


    private Vector2 position;
    private float speed;
    private GameObject owner;

    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            HasPosition = true;
        }
    }

    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            HasSpeed = true;
        }
    }

    public GameObject Owner
    {
        get { return owner; }
        set
        {
            owner = value;
            HasOwner = owner != null;
        }
    }

    public bool HasNothing()
    {
        return !HasPosition && !HasSpeed && !HasOwner;
    }
}
