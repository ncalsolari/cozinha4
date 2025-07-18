using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingounterVisual : MonoBehaviour {
    private const string CUT = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator animator;


    private void Awake() {// no awake nao tem ordem certa de execucao, entao por variavel que depende de ref externa nao eh uma boa, a ref externa pode nao estar inicializada ocasionando erro de refnull
        animator = GetComponent<Animator>();
    }

    private void Start() {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e) {
        animator.SetTrigger(CUT);
    }


}
