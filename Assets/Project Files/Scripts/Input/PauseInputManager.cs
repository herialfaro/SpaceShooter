using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
public class PauseInputManager : MonoBehaviour
{
    #region Fields
    private UnityEngine.InputSystem.PlayerInput playerInput;
    #endregion

    #region Monobehaviour Methods
    private void OnEnable()
    {
        playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.actions["Pause"].started += PlayerPause;
    }

    private void OnDisable()
    {
        playerInput.actions["Pause"].started -= PlayerPause;
    }
    #endregion

    #region Public Methods

    public void PlayerPause(InputAction.CallbackContext context)
    {
        if (context.started)
            SpaceShooterManager.Instance.PlayerPaused();
    }
    #endregion
}
