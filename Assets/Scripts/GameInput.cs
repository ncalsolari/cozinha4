using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {


    public static GameInput Instance { get; private set; }


    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
    }

    private PlayerInputActions playerInputActions;




    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();  //esse .Player e o .Player.Move ali de baixo sao referente as strings dos nomes q foram dados no painel de player interaction no unity

        playerInputActions.Player.Interact.performed += Interact_performed; // o += adiciona um listener (interact_performed) ao evento, por se tratar de um delegate o listener eh uma funcao a ser implementada
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

    }

    private void OnDestroy() { // events static nao sao destruidos nas trocas de cena, o resto das instancias da classe sim. Um evento de uma classe ja destruida pode tentar acessa-la, dando erro de apontar para uma ref null
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty); //trigga um evento (oninteractaction(this, event args)) mas aqui ele testa antes se o evento eh null (n tem nenhum listener) antes de continuar, isso q significa o ?.Invoke
    }
    public UnityEngine.Vector2 GetMovementVectorNormalized() {
        UnityEngine.Vector2 inputVector = playerInputActions.Player.Move.ReadValue<UnityEngine.Vector2>();
        //UnityEngine.Vector2 inputVector = new UnityEngine.Vector2(0, 0); // talvez tenha que usar o outro vector2 System.Numerics.Vector2

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding) {
        playerInputActions.Player.Disable();

        playerInputActions.Player.Move.PerformInteractiveRebinding(1)
        .OnComplete(callback => {
            Debug.Log(callback.action.bindings[1].path);
            Debug.Log(callback.action.bindings[1].overridePath);
            callback.Dispose();
            playerInputActions.Player.Enable();
        }).Start();

    }



}
