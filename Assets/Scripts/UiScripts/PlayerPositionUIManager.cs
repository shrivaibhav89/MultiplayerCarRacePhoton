using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPositionUIManager : MonoBehaviour
{
    public Text playerPositionUIPrefab;
    public Transform playerPositionUIParent;
    public List<Text> playerPositionUIList = new List<Text>();
    public string previousOrder;
    public void CreatePlayerPositionUI(CarTracker car)
    {
        Text playerPositionUI = Instantiate(playerPositionUIPrefab, playerPositionUIParent);
        playerPositionUI.text = car.carCheckpointTracker.name;
        playerPositionUIList.Add(playerPositionUI);
    }
    public void UpdatePlayersPosition()
    {
        if (playerPositionUIList.Count == 0)
        {
            return;
        }
        if (CheckIfPreviousOrderMatchCurrentOrder())
        {
            return;
        }
        
        for (int i = 0; i < RacePositionManager.Instance.cars.Count; i++)
        {
            playerPositionUIList[i].text =  $"{i+1} - {RacePositionManager.Instance.cars[i].carCheckpointTracker.name}";
        }
        Debug.LogError("UpdatePlayersPosition"+previousOrder);
    }

    private bool CheckIfPreviousOrderMatchCurrentOrder()
    {
        string currentOrder = "";
        foreach (CarTracker car in RacePositionManager.Instance.cars)
        {
            currentOrder += car.networkObject.InputAuthority + ",";
        }
        if (previousOrder == currentOrder)
        {
            return true;
        }
        else
        {
            previousOrder = currentOrder;
            return false;
        }
    }
}
