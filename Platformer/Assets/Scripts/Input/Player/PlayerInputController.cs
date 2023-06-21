using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : InputController
{
    [SerializeField]
    private KeyCode jumpKey, attackKey, weaponSwapKey, crouchKey, menuKey;
    public UnityEvent OnMenuKeyPressed;


    private void LateUpdate()
    {
        if (Time.timeScale > 0)
        {
            GetMovementInput();
            inputData.Jump = GetInputState(jumpKey);
            inputData.Attack = GetInputState(attackKey);
            inputData.WeaponSwap = GetInputState(weaponSwapKey);
            inputData.Crouch = GetInputState(crouchKey);
        }

        GetMenuInput();
    }

    private void GetMenuInput()
    {
        if (Input.GetKeyDown(menuKey))
        {
            OnMenuKeyPressed?.Invoke();
        }
    }

    private InputState GetInputState(KeyCode key)
    {
        InputState state = Input.GetKey(key) ? InputState.Held : InputState.Inactive;

        if (Input.GetKeyDown(key))
        {
            state = InputState.Pressed;
        }
        else if (Input.GetKeyUp(key))
        {
            state = InputState.Released;
        }

        return state;
    }

    private void GetMovementInput()
    {
        inputData.MovementVector = GetMovementVector();
    }

    protected Vector2 GetMovementVector()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
