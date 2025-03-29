using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyUiScreen : MonoBehaviour
{
    public Button readyButton;
    public Text raceStatusText;

    private void OnEnable()
    {
        readyButton.onClick.AddListener(OnReadyButtonClicked);
        raceStatusText.text = "Click on Ready to start race";
    }
    private void OnDisable()
    {
        readyButton.onClick.RemoveListener(OnReadyButtonClicked);
    }

    private void OnReadyButtonClicked()
    {
        raceStatusText.text = "Waiting for other players to be ready...";
        GameManager.Instance.userPlayerCar.SetReady();
        readyButton.interactable = false;
    }
   

}
