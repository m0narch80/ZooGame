using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // The player
    public float smoothing = 0.125f;  // Smoothness of movement
    public Vector3 offset;        // Optional: set this in the Inspector

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
