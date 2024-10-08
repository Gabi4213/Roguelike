using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    public Transform pivot;
    public float distanceFromPlayer = 2f;
    public GameObject projSpawnPoint;

    private Vector3 normalProjPos;

    // Reference to the sprite renderer component
    public SpriteRenderer spriteRenderer;

    // Store the direction values
    private float horizontalDirection;
    private float verticalDirection;

    private void Start()
    {
        pivot = transform.parent;
        normalProjPos = projSpawnPoint.transform.position;
    }

    void Update()
    {
        if (InputManager.inventoryOpen) return;

        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, pivot.position.z - Camera.main.transform.position.z));

        // Set the weapon's position to be at a distance from the player
        transform.position = pivot.position + (mousePosition - pivot.position).normalized * distanceFromPlayer;

        // Calculate the angle between the player and the mouse position
        float angle = Mathf.Atan2(mousePosition.y - pivot.position.y, mousePosition.x - pivot.position.x) * Mathf.Rad2Deg;

        // Set the rotation of the weapon instantly
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Flip the sprite vertically if it's in the right half of the circle
        if (angle < 90f && angle > -90f)
        {
            // Flip the sprite vertically
            spriteRenderer.flipY = true;
            projSpawnPoint.transform.localPosition = new Vector3(Mathf.Abs(normalProjPos.x), Mathf.Abs(normalProjPos.y), Mathf.Abs(normalProjPos.z));
        }
        else
        {
            // Reset the sprite flipping
            spriteRenderer.flipY = false;
            projSpawnPoint.transform.localPosition = normalProjPos;
        }

        // Calculate direction for later use
        horizontalDirection = Mathf.Round(Mathf.Cos(angle * Mathf.Deg2Rad));
        verticalDirection = Mathf.Round(Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    // Method to get the direction values
    public Vector2 GetDirection()
    {
        return new Vector2(horizontalDirection, verticalDirection);
    }
}
