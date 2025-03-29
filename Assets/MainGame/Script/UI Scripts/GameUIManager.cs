using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    public PlayerPositionUIManager playerPositionUIManager;
    public GameFinishScreen gameFinishScreen;
    public Text lapText;
    public RaceTimer raceTimer;
    public GameReadyUiScreen gameReadyUiScreen;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GameEventManager.Instance.OnRaceFinish.AddListener(OnRaceFinish);
    }



    private void OnDisable()
    {
        GameEventManager.Instance.OnRaceFinish.RemoveListener(OnRaceFinish);
    }

    private void OnRaceFinish(List<CarTracker> arg0)
    {
        gameFinishScreen.gameObject.SetActive(true);
        gameFinishScreen.OnRaceFinish(arg0);
        raceTimer.StopTimer();
    }
    public void UpdateLapText(int currentLap, int totalLaps)
    {
        lapText.text = $"Lap - {currentLap+1}/{totalLaps}";
    }
    public void EnableGameReadyUiScreen()
    {
        gameReadyUiScreen.gameObject.SetActive(true);
    }
    public void DisableGameReadyUiScreen()
    {
        gameReadyUiScreen.gameObject.SetActive(false);
    }


}
