using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target that the camera will follow
    public Transform target;

    // The delay factor for the camera's movement
    public float delay = 0.3f;

    // The offset of the camera relative to the target
    public Vector3 offset;

    // The velocity of the camera's movement, used by SmoothDamp
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not set for DelayedCameraFollow.");
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Define the target position
            Vector3 targetPosition = target.position + offset;

            // Smoothly move the camera towards the target position with a delay
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, delay);
        }
    }
}
