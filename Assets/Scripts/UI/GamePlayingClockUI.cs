using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Image timerImage;

    private void Update() {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
