using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputController : InputController
{
    private bool jumpKey = false;
    private bool crouchKey = false;
    private bool attackKey = false;
    private bool swapWeaponKey = false;
    private Dictionary<(InputState, bool), InputState> inputFixTable;

    private void Awake()
    {
        inputFixTable = new Dictionary<(InputState, bool), InputState>()
        {
            { (InputState.Inactive, true), InputState.Pressed },
            { (InputState.Pressed, true), InputState.Held },
            { (InputState.Pressed, false), InputState.Released },
            { (InputState.Held, false), InputState.Released },
            { (InputState.Released, true), InputState.Pressed },
            { (InputState.Released, false), InputState.Inactive },
        };
    }


    private void Update()
    {
        inputData.Jump = GetFixedInputState(inputData.Jump, jumpKey);
        inputData.Crouch = GetFixedInputState(inputData.Crouch, crouchKey);
        inputData.Attack = GetFixedInputState(inputData.Attack, attackKey);
        inputData.WeaponSwap = GetFixedInputState(inputData.WeaponSwap, swapWeaponKey);
        attackKey = false;
        swapWeaponKey = false;
    }

    private InputState GetFixedInputState(InputState state, bool keyFlag)
    {
        (InputState, bool) searchKey = (state, keyFlag);
        if (inputFixTable.ContainsKey(searchKey))
        {
            return inputFixTable[searchKey];
        }
        return state;
    }


    public void SetMovementVector(Vector2 movementVector)
    {
        inputData.MovementVector = movementVector;
    }

    public void StartJumping()
    {
        jumpKey = true;
    }

    public void StopJumping()
    {
        jumpKey = false;
    }

    public void StartCrouching()
    {
        crouchKey = true;
    }

    public void StopCrouching()
    {
        crouchKey = false;
    }

    public void Attack()
    {
        attackKey = true;
    }

    public void SwapWeapon()
    {
        swapWeaponKey = true;
    }
}
