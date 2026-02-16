using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    #region Fields
    private PlayerInput playerInput;

    public Vector2 v_Move { get; private set; }
    public bool b_Shoot { get; private set; }
    #endregion

    #region Monobehaviour Methods
    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += PlayerMove;
        playerInput.actions["Shoot"].performed += PlayerIsShooting;
        playerInput.actions["Shoot"].canceled += PlayerStoppedShooting;
        playerInput.actions["Pause"].started += PlayerMove;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= PlayerMove;
        playerInput.actions["Shoot"].performed -= PlayerIsShooting;
        playerInput.actions["Shoot"].canceled -= PlayerStoppedShooting;
        playerInput.actions["Pause"].started -= PlayerMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Public Methods
    public void PlayerMove(InputAction.CallbackContext context)
    {
        v_Move = context.ReadValue<Vector2>();
    }

    public void PlayerPause(InputAction.CallbackContext context)
    {
        if (context.started)
            SpaceShooterManager.Instance.PlayerPaused();
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
