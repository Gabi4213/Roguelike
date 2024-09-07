using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    private Transform pivot;

    private void Start()
    {
        pivot = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (InputManager.inventoryOpen) return;

        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, pivot.position.z - Camera.main.transform.position.z));

        // Calculate the angle between the player and the mouse position
        float angle = Mathf.Atan2(mousePosition.y - pivot.position.y, mousePosition.x - pivot.position.x) * Mathf.Rad2Deg;

        // Set the rotation of the weapon instantly
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
