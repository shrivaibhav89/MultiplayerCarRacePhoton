using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

public class GameManager : MonoBehaviour
{
    
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public static GameManager Instance { get; private set; }
    public string playerName;

    private void OnEnable()
    {
        GameEventManager.Instance.OnRaceFinish.AddListener(OnRaceFinish);
    }
    private void OnDisable()
    {
        GameEventManager.Instance.OnRaceFinish.RemoveListener(OnRaceFinish);
    }




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

    public void SetFollowTarget(Transform target)
    {
        cinemachineVirtualCamera.Follow = target;
        cinemachineVirtualCamera.LookAt = target;
    }
    private void OnRaceFinish(List<CarTracker> cars)
    {
        Debug.Log("Winner of Race from Game Manager " + cars[0].carCheckpointTracker.name);
        StopControllForPlayerCar(cars);
    }
    private void StopControllForPlayerCar(List<CarTracker> cars)
    {
        foreach (CarTracker car in cars)
        {
            if(car.isMine)
            {
                car.carCheckpointTracker.gameObject.GetComponent<UserControl>().enabled = false;
                 car.carCheckpointTracker.gameObject.GetComponent<NetworkRigidbody3D>().enabled = false;
                // Rigidbody rigidbody = car.carCheckpointTracker.gameObject.GetComponent<Rigidbody>();
                // rigidbody.velocity = Vector3.zero;
                // rigidbody.angularVelocity = Vector3.zero;
                // rigidbody.isKinematic = true;
            }
        }
        //stop photon connection 
       
    }
}
