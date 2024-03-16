using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    // declare event handler
    public event EventHandler OnInteractAction;

    private PlayerInputActions playerInputActions;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += InteractionPerformed;
    }

    private void InteractionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // call event handler... EventArgs.Empty is there to keep errors from throwing because it expects a second argument.
        // event handler BY DEFAULT will throw an error (NullReferenceException) UNLESS you have subscribers to this EventHandler (see Player.cs).
        // question mark ? is a Null Conditional Operator - only continues with call if it's not null. we need .Invoke after the question mark because there cannot be parentheses directly after the question mark.
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        /* 
        // HOW IS IT THIS EASY TO REFACTOR BETWEEN INPUT SYSTEMS
        // I SPENT THREE WEEKS FACTORING CODE FOR THE NEW INPUT SYSTEM
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }
        */

        inputVector = inputVector.normalized;
        return inputVector;
    }
}
