using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinishScreen : MonoBehaviour
{
    public Transform racePositionParent;
    public GameObject racePositionPrefab;

    public void OnRaceFinish(List<CarTracker> cars)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            string isYou = cars[i].isMine==true?"(You)":"";
            GameObject racePosition = Instantiate(racePositionPrefab, racePositionParent);
            racePosition.GetComponentInChildren<Text>().text = $"{i + 1} - {cars[i].carCheckpointTracker.name} {isYou}";
        }
    }
}
