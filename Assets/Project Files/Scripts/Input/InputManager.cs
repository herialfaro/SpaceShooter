using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
public class InputManager : MonoBehaviour
{
    #region Fields
    private UnityEngine.InputSystem.PlayerInput playerInput;

    public Vector2 v_Move { get; private set; }
    public bool b_Shoot { get; private set; }
    #endregion

    #region Monobehaviour Methods
    private void OnEnable()
    {
        playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.actions["Move"].performed += PlayerMove;
        playerInput.actions["Shoot"].performed += PlayerIsShooting;
        playerInput.actions["Shoot"].canceled += PlayerStoppedShooting;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= PlayerMove;
        playerInput.actions["Shoot"].performed -= PlayerIsShooting;
        playerInput.actions["Shoot"].canceled -= PlayerStoppedShooting;
    }
    #endregion

    #region Public Methods
    public void PlayerMove(InputAction.CallbackContext context)
    {
        v_Move = context.ReadValue<Vector2>();
    }

    public void PlayerIsShooting(InputAction.CallbackContext context)
    {
        if (context.performed)
            b_Shoot = true;
    }

    public void PlayerStoppedShooting(InputAction.CallbackContext context)
    {
        if (context.canceled)
            b_Shoot = false;
    }
    #endregion
}
