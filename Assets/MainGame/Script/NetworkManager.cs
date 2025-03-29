using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private NetworkRunner _networkRunner;

    private void Start()
    {
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;

        _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "CarRaceRoom",
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>() // Use the default scene manager
        });
    }

}
