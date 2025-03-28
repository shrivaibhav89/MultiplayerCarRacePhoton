using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
   
    [SerializeField] private Transform sawnPosition;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private int spawnPlayerCount = 0;
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        spawnPlayerCount++;
        Transform spawnPoint = sawnPosition.GetChild(spawnPlayerCount);
        //Transform playerTransform = null;
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 8, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab,spawnPoint.position , spawnPoint.rotation, player);
            _spawnedCharacters.Add(player, networkPlayerObject);

        }

        StartCoroutine(DelayedPlayerSync(runner, player));
        Debug.LogError("OnPlayerJoined" + runner.IsClient + " And  " + player.RawEncoded);

    }
    private IEnumerator DelayedPlayerSync(NetworkRunner Runner, PlayerRef player)
    {
        yield return new WaitForSeconds(1f);  // Slight delay to allow spawning

        NetworkObject[] allObjects = FindObjectsOfType<NetworkObject>();
        foreach (var obj in allObjects)
        {
            if (obj.gameObject.TryGetComponent(out CarCheckpointTracker carCheckpointTracker))
            {
                obj.gameObject.name = carCheckpointTracker.PlayerName.ToString();
                RacePositionManager.Instance.AddNewCar(new CarTracker()
                {
                    carCheckpointTracker = carCheckpointTracker,
                    racePosition = 0,
                    isMine = obj.HasInputAuthority,
                    networkObject = obj
                });
            }
            if (obj.HasInputAuthority)
            {
               // obj.name = " local Player " + obj.Runner.LocalPlayer.RawEncoded;
                if (obj.Runner.IsClient)
                {
                    Runner.SetIsSimulated(obj, false);
                }
                else
                {
                    Runner.SetIsSimulated(obj, true);
                }
                //  _spawnedCharacters[player] = obj;
                GameManager.Instance.SetFollowTarget(obj.transform);
                // Debug.Log($"Player object assigned for PlayerRef {player.RawEncoded} on client.");
            }
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
        {
            // data.direction += Vector3.forward;
            data.vertical = 1;
        }


        if (Input.GetKey(KeyCode.S))
        {
            // data.direction += Vector3.back;
            data.vertical = -1;
        }


        if (Input.GetKey(KeyCode.A))
        {
            // data.direction += Vector3.left;
            data.horizontal = -1;
        }


        if (Input.GetKey(KeyCode.D))
        {
            // data.direction += Vector3.right;
            data.horizontal = 1;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            data.handBreak = true;
        }

        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    private NetworkRunner _runner;

    private void OnEnable()
    {
        GameEventManager.Instance.OnRaceFinish.AddListener(OnRaceFinish);
    }
    private void OnDisable()
    {
        GameEventManager.Instance.OnRaceFinish.RemoveListener(OnRaceFinish);
    }

    private void OnRaceFinish(List<CarTracker> cars)
    {
       //_runner.Shutdown();
    }


    public async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}