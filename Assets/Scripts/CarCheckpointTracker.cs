using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class CarCheckpointTracker : NetworkBehaviour
{
    [HideInInspector] public int currentCheckpointIndex = 0;
    [Networked] public NetworkString<_16> PlayerName { get; set; }
    [Networked, OnChangedRender(nameof(OnRaceFinsihed))] public NetworkBool IsRaceFinished { get; set; }

    [Networked, OnChangedRender(nameof(OnLapCountChanged))]
    public int LapCount { get; set; }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // Set player's name when they join
            RPC_SetPlayerName(GameManager.Instance.playerName);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetPlayerName(string newName)
    {
        PlayerName = newName;
        Debug.Log($"Player name set to {newName}");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Runner.IsServer)
        {
            return;
        }
        if (other.CompareTag("Checkpoint")) // Make sure checkpoints have this tag!
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            CheckForFinsihLine(checkpoint);
            if (checkpoint.index == currentCheckpointIndex + 1) // Must pass checkpoints in order
            {
                currentCheckpointIndex = checkpoint.index;
                Debug.Log($"{gameObject.name} reached checkpoint {currentCheckpointIndex}");
            }
        }
    }
    public void CheckForFinsihLine(Checkpoint checkpoint)
    {
        if (checkpoint.index == 0 && currentCheckpointIndex == CheckpointManager.Instance.GetTotalCheckpoints() - 1)
        {
            currentCheckpointIndex = 0;
            RacePositionManager.Instance.AddLapForCar(this);
            Debug.Log($"{gameObject.name} completed lap ");
        }
    }
    public void RaceFinishClient(CarTracker car)
    {
        IsRaceFinished = true;
    }
    public void UpdateLapCount(int lap)
    {
        if (Object.HasStateAuthority) // Only the Host can modify
        {
            LapCount = lap;
        }
    }
    public void OnLapCountChanged()
    {
        if (Runner.IsClient)
        {
            GameUIManager.Instance.UpdateLapText(LapCount, RacePositionManager.Instance.totalNumberOfLaps);
        }
    }

    private void OnRaceFinsihed()
    {
        if (Runner.IsClient)
        {
            if (IsRaceFinished )
            {
                GameEventManager.Instance.FinishRace(RacePositionManager.Instance.cars);
            }
        }
    }

    public bool HasPassedAllCheckpoints()
    {
        return currentCheckpointIndex >= CheckpointManager.Instance.GetTotalCheckpoints();
    }
}
