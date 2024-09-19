using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : MonoBehaviour
{
    public GameObject topWall;    // The top wall of the room
    public GameObject bottomWall; // The bottom wall of the room
    public GameObject leftWall;   // The left wall of the room
    public GameObject rightWall;  // The right wall of the room

    public Vector2 roomSize;  // Automatically calculated room size based on wall positions
    public Vector2Int gridPosition;  // Position of the room on the grid

    public bool hasNorthDoor, hasSouthDoor, hasWestDoor, hasEastDoor;

    public GameObject northDoor;  // Door GameObjects
    public GameObject southDoor;
    public GameObject westDoor;
    public GameObject eastDoor;

    public float margin = 0.1f;  // Margin to adjust room size

    void Start()
    {
        CalculateRoomSize();  // Calculate the room size when the room is initialized
    }

    void CalculateRoomSize()
    {
        // Ensure all walls are assigned before calculating the room size
        if (topWall != null && bottomWall != null && leftWall != null && rightWall != null)
        {
            // Calculate room width and height using wall positions
            float width = Mathf.Abs(rightWall.transform.position.x - leftWall.transform.position.x) + margin;
            float height = Mathf.Abs(topWall.transform.position.y - bottomWall.transform.position.y) + margin;

            // Set the room size
            roomSize = new Vector2(width, height);

            Debug.Log("Room size calculated with margin: " + roomSize);
        }
        else
        {
            Debug.LogWarning("One or more walls are not assigned in Room2.");
        }
    }

    public void ConnectRoom(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            hasNorthDoor = true;
            northDoor.SetActive(true);
        }
        else if (direction == Vector2Int.down)
        {
            hasSouthDoor = true;
            southDoor.SetActive(true);
        }
        else if (direction == Vector2Int.left)
        {
            hasWestDoor = true;
            westDoor.SetActive(true);
        }
        else if (direction == Vector2Int.right)
        {
            hasEastDoor = true;
            eastDoor.SetActive(true);
        }
    }

    public void DeactivateAllDoors()
    {
        northDoor.SetActive(false);
        southDoor.SetActive(false);
        westDoor.SetActive(false);
        eastDoor.SetActive(false);
    }
}
