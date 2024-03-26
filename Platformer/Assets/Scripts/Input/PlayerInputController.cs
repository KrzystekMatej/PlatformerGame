using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : InputController
{
    [SerializeField]
    private KeyCode rightKey, leftKey, upKey, downKey, jumpKey, attackKey, weaponSwapKey, menuKey;
    public UnityEvent OnMenuKeyPressed;

    private WeaponManager weaponManager;

    private void Awake()
    {
        weaponManager = GetComponentInChildren<WeaponManager>();
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            inputData.SteeringForce = GetSteeringForce();
            inputData.Jump = GetInputState(jumpKey);
            inputData.Attack = GetInputState(attackKey);
            if (GetInputState(weaponSwapKey) == InputState.Pressed) weaponManager.SwapWeapon();
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

    private Vector2 GetSteeringForce()
    {
        //return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * agent.InstanceData.MaxForce; - for stop if both keys are down
        Vector2 currentMovementVector = MathUtility.GetSignedVector(inputData.SteeringForce);
        Vector2 newMovementVector = new Vector2
        (
            GetMovementVectorComponent(rightKey, leftKey, currentMovementVector.x),
            GetMovementVectorComponent(upKey, downKey, currentMovementVector.y)
        );

        DecelerationFlags.x = (DecelerationFlags.x && newMovementVector.x == 0) || (currentMovementVector.x != 0 && newMovementVector.x == 0);
        DecelerationFlags.y = (DecelerationFlags.y && newMovementVector.y == 0) || (currentMovementVector.y != 0 && newMovementVector.y == 0);

        return newMovementVector.normalized * instanceData.MaxForce;
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
