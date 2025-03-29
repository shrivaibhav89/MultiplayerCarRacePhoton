using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFinishScreen : MonoBehaviour
{
    public Transform racePositionParent;
    public GameObject racePositionPrefab;
    public Text timeToFinishText;
    public Button closeButton;
    private void OnEnable()
    {
        closeButton.onClick.AddListener(OnCLoseButtonCLick);
    }
    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(OnCLoseButtonCLick);
    }

    private void OnCLoseButtonCLick()
    {
       Application.Quit();
    }
    public void OnRaceFinish(List<CarTracker> cars)
    {
        cars.Sort((car1, car2) =>
        {
            // First, check lap count
            if (car1.lapCount != car2.lapCount)
                return car2.lapCount.CompareTo(car1.lapCount);

            // Next, check checkpoint index (so players can't skip)
            if (car1.carCheckpointTracker.currentCheckpointIndex != car2.carCheckpointTracker.currentCheckpointIndex)
                return car2.carCheckpointTracker.currentCheckpointIndex.CompareTo(car1.carCheckpointTracker.currentCheckpointIndex);

            // Finally, compare progress along the spline
            return car2.progress.CompareTo(car1.progress);
        });
        foreach (Transform ts in racePositionParent)
        {
            Destroy(ts.gameObject);
        }
        for (int i = 0; i < cars.Count; i++)
        {
            string isYou = cars[i].isMine == true ? "(You)" : "";
            GameObject racePosition = Instantiate(racePositionPrefab, racePositionParent);
            racePosition.GetComponentInChildren<Text>().text = $"{i + 1} - {cars[i].carCheckpointTracker.name} {isYou}";
        }
        float minutes = Mathf.FloorToInt(GameManager.Instance.raceDuration / 60);
        float seconds = Mathf.FloorToInt(GameManager.Instance.raceDuration % 60);
        float milliseconds = Mathf.FloorToInt((GameManager.Instance.raceDuration * 1000) % 1000);
        timeToFinishText.text = "Time taken to finish Race " + string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
