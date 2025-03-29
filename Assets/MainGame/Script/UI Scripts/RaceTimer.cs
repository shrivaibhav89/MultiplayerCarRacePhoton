using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimer : MonoBehaviour
{
    public Text timerText;
    private float raceTime;
    public float currentTime{ get; private set; }
    public bool isTimerRunning;
   
    public void StartRace()
    {
        isTimerRunning = true;
        raceTime = GameManager.Instance.totalRaceTimeInSeconds;
        StartCoroutine(UpdateTimer());
    }
    public void StopTimer()
    {
        isTimerRunning = false;
    }
    public IEnumerator UpdateTimer()
    {
        float timeLeft = raceTime;
        while (timeLeft>0 && isTimerRunning)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText(timeLeft);
            currentTime = timeLeft;
            yield return null;
        }
       /// timeLeft = 0;
        UpdateTimerText(timeLeft);
        GameEventManager.Instance.OnRaceFinish.Invoke(RacePositionManager.Instance.cars);
    }
    private void UpdateTimerText(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        int milliseconds = Mathf.FloorToInt((timeLeft * 1000) % 1000);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
