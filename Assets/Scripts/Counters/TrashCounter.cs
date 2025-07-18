using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter {

    public static EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData() { // o new aqui eh pq essa classe extende de Basecounter q tbm tem uma func dessa entao a gnt ta escondendo a func do pai explicitamente cm esse new. tira o new e veja o warning q aparece p + infos
        OnAnyObjectTrashed = null;
    }
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            player.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
