using UnityEngine;
using System.Collections.Generic;

public class Room4 : MonoBehaviour
{
    // Lists to store multiple adjacent rooms for each side
    public List<Room4> leftRooms = new List<Room4>();
    public List<Room4> rightRooms = new List<Room4>();
    public List<Room4> topRooms = new List<Room4>();
    public List<Room4> bottomRooms = new List<Room4>();

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
                rightDoors[i].SetActive(true);
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
            }
            else
            {
                bottomDoors[i].SetActive(false);
            }
        }

        // Enable doors based on actual adjacency
        EnableDoorsBasedOnAdjacency();
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
