using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour {
    private const string OPNE_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;

    private Animator animator;


    private void Awake() {// no awake nao tem ordem certa de execucao, entao por variavel que depende de ref externa nao eh uma boa, a ref externa pode nao estar inicializada ocasionando erro de refnull
        animator = GetComponent<Animator>();
    }

    private void Start() {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) {
        animator.SetTrigger(OPNE_CLOSE);
    }


}
