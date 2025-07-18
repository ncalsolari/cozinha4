using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;


    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError("Gmae Object" + hasProgressGameObject + "does not have a component that omplements IHasProgress!");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;//ouvindo o evento

        barImage.fillAmount = 0f;

        Hide(); // hide depois de ouvir o evento, se desativar o gameobjt antes ele nunca definido como ouvinte do evento
    }
    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        }
        else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}


