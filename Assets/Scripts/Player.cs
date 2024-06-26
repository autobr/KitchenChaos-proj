using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; set; }


    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractionDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null) { Debug.LogError("More than one Player Instance!");  }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // subscribe to OnInteractAction
        gameInput.OnInteractAction += PlayerOnInteractAction;
        gameInput.OnInteractAlternateAction += PlayerOnInteractAlternateAction;
    }

    private void PlayerOnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void PlayerOnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) 
        {
            selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);

            }
        }
        else
        {
            SetSelectedCounter(null);
        }
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
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // can move on x
                moveDir = moveDirX;
            }
            else
            {
                // attempt z dir movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirZ, moveDistance);

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

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransform() { return kitchenObjectHoldPoint; }
    public void SetKitchenObject(KitchenObject kitchenObject) 
    {
        this.kitchenObject = kitchenObject; 

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
