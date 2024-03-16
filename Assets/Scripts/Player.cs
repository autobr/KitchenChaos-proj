using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    private bool isWalking;
    private Vector3 lastInteractionDir;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to OnInteractAction
        gameInput.OnInteractAction += PlayerOnInteractAction;
    }

    private void PlayerOnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // has clearcounter
                clearCounter.Interact();
            }
        }
        else { }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        // HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // has clearcounter
                clearCounter.Interact();
            }
        } else { }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // separate logic as much as possible. inputVector could just be a Vector3 right out the gate, but since you're only using two axes on your keyboard, we convert down below.
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.6f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // cannot move on moveDir

            // attempt only x direction movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // can move on x
                moveDir = moveDirX;
            }
            else
            {
                // attempt z dir movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // can move on z
                    moveDir = moveDirZ;
                }
                else { /* can't move (haha loser) */ }
            }
        }



        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;
            // print("canMove = " + canMove);
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
