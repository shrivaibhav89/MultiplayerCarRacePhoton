using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
    public RaceFinishEvent OnRaceFinish;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       if (OnRaceFinish == null)
        OnRaceFinish = new RaceFinishEvent(new List<CarTracker>());
    }
    public void FinishRace(List<CarTracker> cars)
    {
        Debug.Log($"Race finish fuction called");
        OnRaceFinish?.Invoke(cars); // Trigger the event
    }

}
public class RaceFinishEvent : UnityEngine.Events.UnityEvent<List<CarTracker>>
{
    public List<CarTracker> cars;
    public RaceFinishEvent(List<CarTracker> cars)
    {
        this.cars = new List<CarTracker>();
        this.cars = cars;
        // Add your code here
    }

}