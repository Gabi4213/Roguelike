using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Define a variable to store the rotation speed
    public float rotationSpeed = 50f;

    // Define a variable to store the rotation direction
    public Vector3 rotationDirection = Vector3.up; // Default is rotating around the y-axis

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around the chosen axis
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
    }
}
