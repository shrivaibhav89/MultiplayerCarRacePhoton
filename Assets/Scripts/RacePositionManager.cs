using System.Collections.Generic;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines; // If using Unity's Spline System

public class RacePositionManager : NetworkBehaviour
{
    public SplineContainer splineContainer; // Assign your track spline
    public List<CarTracker> cars; // Assign all car trackers
    public static RacePositionManager Instance;
    public int totalNumberOfLaps = 2;

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
    }
    public void AddNewCar(CarTracker carTracker)
    {
        // i want to check in cars if a object with same  car.carCheckpointTracker then return 
        // else add to cars
        if (cars.Find(x => x.carCheckpointTracker == carTracker.carCheckpointTracker) != null)
        {
            return;
        }
        cars.Add(carTracker);
        GameUIManager.Instance.playerPositionUIManager.CreatePlayerPositionUI(carTracker);
        GameUIManager.Instance.UpdateLapText(carTracker.lapCount, totalNumberOfLaps);
    }

    public void AddLapForCar(CarCheckpointTracker carCheckpointTracker)
    {
        CarTracker car = cars.Find(x => x.carCheckpointTracker == carCheckpointTracker);
        if (car != null)
        {
            car.lapCount++;
        }
        if (car.isMine)
        {
            GameUIManager.Instance.UpdateLapText(car.lapCount, totalNumberOfLaps);
        }
        car.carCheckpointTracker.UpdateLapCount(car.lapCount);
        CheckForRaceFinish(car);
    }

    public void CheckForRaceFinish(CarTracker car)
    {
        if (car.lapCount == totalNumberOfLaps)
        {
            RaceFinishForPlayer(car);
        }
    }

    public void RaceFinishForPlayer(CarTracker car)
    {
        // Debug.LogError("Rce finish for player" + car.carCheckpointTracker.PlayerName);
        if (car.isMine)
        {
            GameEventManager.Instance.FinishRace(cars);
        }
        car.carCheckpointTracker.RaceFinishClient(car);

    }
    public CarTracker GetCarTracker(CarCheckpointTracker carCheckpointTracker)
    {
        return cars.Find(x => x.carCheckpointTracker == carCheckpointTracker);
    }
    void Update()
    {
        if (cars.Count == 0)
        {
            return;
        }
        foreach (var car in cars)
        {
            car.UpdateProgress(splineContainer);
        }

        // Sort cars based on lap count first, then spline position
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

        // Assign race positions
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].racePosition = i + 1;
        }
        GameUIManager.Instance.playerPositionUIManager.UpdatePlayersPosition();
    }

}

[System.Serializable]
public class CarTracker
{
    public CarCheckpointTracker carCheckpointTracker;
    // public Transform carTransform;
    public int lapCount = 0; // Track laps
    public float progress = 0f; // Store progress on spline
    public int racePosition = 1; // 1st, 2nd, 3rd, etc.
    public bool isMine = false;
    public NetworkObject networkObject;
    public void UpdateProgress(SplineContainer splineContainer)
    {
        progress = GetClosestPointOnSpline(carCheckpointTracker.transform.position, splineContainer);
        //lastProgress = progress;
    }

    float GetClosestPointOnSpline(Vector3 carPosition, SplineContainer splineContainer)
    {
        // int sampleSteps = 100;
        // float closestT = lastProgress;  // Start search near the last progress
        // float minDistance = float.MaxValue;

        Spline spline = splineContainer.Splines[0];
        float3 localCarPosition = splineContainer.transform.InverseTransformPoint(carPosition);

        int resolution = 5;
        int iterations = 2;

        float3 nearestPoint;
        float t;
        SplineUtility.GetNearestPoint(spline, localCarPosition, out nearestPoint, out t, resolution, iterations);
        float distanceAlongSpline = t * splineContainer.CalculateLength(0);
        return distanceAlongSpline;
    }
}
