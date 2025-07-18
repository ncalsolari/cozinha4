using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }



    [SerializeField] private float moveSpeed = 7f; // o serializefield deixa a gnt mexer na variavel pelo unity msmo ela sendo private
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    private bool isWalking;
    private UnityEngine.Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }


    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }

    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }

    }

    // Update is called once per frame
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement() {

        UnityEngine.Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        // o ideal eh separar a logica da movimentacao, primeito solucionar os inputs e depois lidar com a mov do objeto

        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        //n gosto dessa solucao toda do canMove nao, copiei igual
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (UnityEngine.Vector3.up * playerHeight), playerRadius, moveDir, moveDistance);
        // Debug.Log("canMove: " + canMove);

        if (!canMove) {
            //Cannot move towards moveDir

            //Attempt only X movement
            UnityEngine.Vector3 moveDirX = new UnityEngine.Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + (UnityEngine.Vector3.up * playerHeight), playerRadius, moveDirX, moveDistance);

            if (canMove) {
                //Can move only the X
                moveDir = moveDirX;

            }
            else {
                //Cannot move only on the X

                //Attempt only Z movement
                UnityEngine.Vector3 moveDirZ = new UnityEngine.Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + (UnityEngine.Vector3.up * playerHeight), playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    // Can move only on the Z
                    moveDir = moveDirZ;

                }
                else {
                    // Cannot move any direction
                }
            }



        }

        if (canMove) {
            transform.position += moveDir * moveDistance; // time delta time deixa o movimento independente do fp
        }

        isWalking = moveDir != UnityEngine.Vector3.zero;
        float rotateSpeed = 10f;
        // slerp interpola entre 2 posicoes, deixando a transicao mais smooth, o time aqui é a terceira variavel q meio q indica a velocidade dessa interpolacao
        transform.forward = UnityEngine.Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);


    }



    private void HandleInteractions() {

        UnityEngine.Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != UnityEngine.Vector3.zero) {
            lastInteractDir = moveDir;
        }


        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
                //Has ClearCounter
                //clearCounter.Interact();
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }

    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });

    }

    public bool IsWalking() {
        return isWalking;
    }

    public Transform GetKitchenObjectFollowTrasform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
