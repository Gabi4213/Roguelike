using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    // Lists to store multiple adjacent rooms for each side
    public List<Room> leftRooms = new List<Room>();
    public List<Room> rightRooms = new List<Room>();
    public List<Room> topRooms = new List<Room>();
    public List<Room> bottomRooms = new List<Room>();

    // Lists of door GameObjects for each side
    // Ensure that each list has enough door GameObjects in the Unity Editor
    public List<GameObject> leftDoors;
    public List<GameObject> rightDoors;
    public List<GameObject> topDoors;
    public List<GameObject> bottomDoors;

    public void UpdateDoors()
    {
        // Update Left Doors
        for (int i = 0; i < leftDoors.Count; i++)
        {
            if (i < leftRooms.Count)
            {
                leftDoors[i].SetActive(true);
                // Set the adjacent door
                Door leftDoorScript = leftDoors[i].GetComponent<Door>();
                if (leftDoorScript != null && leftRooms[i] != null)
                {
                    // Check if the neighbor has right doors
                    if (leftRooms[i].rightDoors.Count > 0)
                    {
                        Door adjacentLeftDoor = leftRooms[i].rightDoors[0].GetComponent<Door>(); // Assuming the first door
                        leftDoorScript.adjacentDoor = adjacentLeftDoor;
                    }
                }
            }
            else
            {
                leftDoors[i].SetActive(false);
            }
        }

        // Update Right Doors
        for (int i = 0; i < rightDoors.Count; i++)
        {
            if (i < rightRooms.Count)
            {
                if (rightDoors[i])
                {
                    rightDoors[i].SetActive(true);
                    // Set the adjacent door
                    Door rightDoorScript = rightDoors[i].GetComponent<Door>();
                    if (rightDoorScript != null && rightRooms[i] != null)
                    {
                        if (rightRooms[i].leftDoors.Count > 0)
                        {
                            Door adjacentRightDoor = rightRooms[i].leftDoors[0].GetComponent<Door>();
                            rightDoorScript.adjacentDoor = adjacentRightDoor;
                        }
                    }
                }
            }
            else
            {
                rightDoors[i].SetActive(false);
            }
        }

        // Update Top Doors
        for (int i = 0; i < topDoors.Count; i++)
        {
            if (i < topRooms.Count)
            {
                topDoors[i].SetActive(true);
                // Set the adjacent door
                Door topDoorScript = topDoors[i].GetComponent<Door>();
                if (topDoorScript != null && topRooms[i] != null)
                {
                    if (topRooms[i].bottomDoors.Count > 0)
                    {
                        Door adjacentTopDoor = topRooms[i].bottomDoors[0].GetComponent<Door>();
                        topDoorScript.adjacentDoor = adjacentTopDoor;
                    }
                }
            }
            else
            {
                topDoors[i].SetActive(false);
            }
        }

        // Update Bottom Doors
        for (int i = 0; i < bottomDoors.Count; i++)
        {
            if (i < bottomRooms.Count)
            {
                bottomDoors[i].SetActive(true);
                // Set the adjacent door
                Door bottomDoorScript = bottomDoors[i].GetComponent<Door>();
                if (bottomDoorScript != null && bottomRooms[i] != null)
                {
                    if (bottomRooms[i].topDoors.Count > 0)
                    {
                        Door adjacentBottomDoor = bottomRooms[i].topDoors[0].GetComponent<Door>();
                        bottomDoorScript.adjacentDoor = adjacentBottomDoor;
                    }
                }
            }
            else
            {
                bottomDoors[i].SetActive(false);
            }
        }
    }


    private void EnableDoorsBasedOnAdjacency()
    {
        // Reset all doors to inactive first
        foreach (var door in leftDoors) door.SetActive(false);
        foreach (var door in rightDoors) door.SetActive(false);
        foreach (var door in topDoors) door.SetActive(false);
        foreach (var door in bottomDoors) door.SetActive(false);

        // Enable doors based on the adjacency lists
        foreach (var neighbor in leftRooms)
        {
            if (neighbor != null)
            {
                int index = leftRooms.IndexOf(neighbor);
                if (index >= 0 && index < leftDoors.Count)
                {
                    leftDoors[index].SetActive(true);
                }
            }
        }

        foreach (var neighbor in rightRooms)
        {
            if (neighbor != null)
            {
                int index = rightRooms.IndexOf(neighbor);
                if (index >= 0 && index < rightDoors.Count)
                {
                    rightDoors[index].SetActive(true);
                }
            }
        }

        foreach (var neighbor in topRooms)
        {
            if (neighbor != null)
            {
                int index = topRooms.IndexOf(neighbor);
                if (index >= 0 && index < topDoors.Count)
                {
                    topDoors[index].SetActive(true);
                }
            }
        }

        foreach (var neighbor in bottomRooms)
        {
            if (neighbor != null)
            {
                int index = bottomRooms.IndexOf(neighbor);
                if (index >= 0 && index < bottomDoors.Count)
                {
                    bottomDoors[index].SetActive(true);
                }
            }
        }
    }

}
