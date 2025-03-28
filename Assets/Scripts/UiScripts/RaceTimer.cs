using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimer : MonoBehaviour
{
    public Text timerText;
    public float raceTime = 30;
    public void StartRace()
    {
        StartCoroutine(UpdateTimer());
    }
    public IEnumerator UpdateTimer()
    {
        float timeLeft = raceTime;
        while (timeLeft>0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText(timeLeft);
            yield return null;
        }
        timeLeft = 0;
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
