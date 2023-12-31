using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : InputController
{
    [SerializeField]
    private KeyCode rightKey, leftKey, upKey, downKey, jumpKey, attackKey, weaponSwapKey, crouchKey, menuKey;
    public UnityEvent OnMenuKeyPressed;


    private void Update()
    {
        if (Time.timeScale > 0)
        {
            inputData.MovementVector = GetMovementVector();
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

    private Vector2 GetMovementVector()
    {
        //return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); - for stop if both keys are down
        return new Vector2(GetMovementVectorComponent(rightKey, leftKey, inputData.MovementVector.x), GetMovementVectorComponent(upKey, downKey, inputData.MovementVector.y));
    }

    //for last key priority
    private float GetMovementVectorComponent(KeyCode positiveKey, KeyCode negativeKey, float previousValue)
    {
        if (Input.GetKeyDown(positiveKey)) return 1;
        else if (Input.GetKeyDown(negativeKey)) return -1;

        if (Input.GetKeyUp(positiveKey)) return Input.GetKey(negativeKey) ? -1 : 0;
        else if (Input.GetKeyUp(negativeKey)) return Input.GetKey(positiveKey) ? 1 : 0;

        return previousValue;
    }
}
