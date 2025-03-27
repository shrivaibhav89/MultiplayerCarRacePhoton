using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Reference to the car to follow
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Camera position offset
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothness of the follow


    private void FixedUpdate()
    {
        if (target == null)
            return;

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;

        // Smooth the position using Lerp
        transform.position = Vector3.Lerp(transform.position, desiredPosition,  Time.fixedDeltaTime * smoothSpeed);

        // Optional: Look at the target
        transform.LookAt(target);
    }
    // Call this method to set the target dynamically
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
