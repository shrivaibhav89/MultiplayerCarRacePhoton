using Fusion;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public NetworkPrefabRef carPrefab;

    private void Start()
    {
        NetworkRunner runner = FindObjectOfType<NetworkRunner>();
        runner.Spawn(carPrefab, transform.position, Quaternion.identity, runner.LocalPlayer);
    }
}
