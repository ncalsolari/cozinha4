using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;




    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            // Player is not carring anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player); //eh uma func static entao chamo a classe e nao uma instancia da classe

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }



}