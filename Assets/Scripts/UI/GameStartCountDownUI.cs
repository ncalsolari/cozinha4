using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI countdownText;


    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChange;

        Hide();
    }

    private void Update() {
        countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Show();

        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
