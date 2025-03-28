using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance; // Singleton for easy access

    public Transform[] checkpoints; // Assign these in the Inspector

    private void Awake()
    {
        Instance = this;
    }

    public int GetTotalCheckpoints()
    {
        return checkpoints.Length;
    }
}
