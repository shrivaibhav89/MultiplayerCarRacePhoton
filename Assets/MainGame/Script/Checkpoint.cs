using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index; // Set this manually in Inspector (0, 1, 2, ...)

    private void Start()
    {
        gameObject.tag = "Checkpoint"; // Ensures trigger detection works
    }
}
