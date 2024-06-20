using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //instance of this script
    public static CameraFollow instance;

    // The target that the camera will follow
    public Transform target;

    // The delay factor for the camera's movement
    public float delay = 0.3f;

    // The offset of the camera relative to the target
    public Vector3 offset;

    // The velocity of the camera's movement, used by SmoothDamp
    private Vector3 velocity = Vector3.zero;

    // Duration and magnitude of the camera shake
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    // Coroutine for camera shake
    private Coroutine shakeCoroutine;

    void Start()
    {
        if (!instance)
        {
            instance = this;
        }

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

    public void ShakeCamera()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
