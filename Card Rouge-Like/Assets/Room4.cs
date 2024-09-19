using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room4 : MonoBehaviour
{
    public GameObject topDoor, bottomDoor, leftDoor, rightDoor; // Doors for 1x1 rooms
    public GameObject[] extraDoors; // Extra doors for larger rooms like 2x1 or 2x2

    // Function to activate the appropriate door based on direction
    public void ActivateDoor(Vector2Int direction)
    {

    }
}
