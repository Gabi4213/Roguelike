using UnityEngine;

using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomDoor doorTop, doorBottom, doorLeft, doorRight;

    // New variables to track connected rooms
    public Room roomAbove;
    public Room roomBelow;
    public Room roomLeft;
    public Room roomRight;

    private void Start()
    {
        if (roomAbove)
        {
            doorTop.connectedRoom = roomAbove;
        }
        if (roomBelow)
        {
            doorBottom.connectedRoom = roomBelow;
        }
        if (roomLeft)
        {
            doorLeft.connectedRoom = roomLeft;
        }
        if (roomRight)
        {
            doorRight.connectedRoom = roomRight;
        }
    }

    // Method to check if this room has an available spot in a given direction
    public bool HasAvailableDirection(RoomDirection direction)
    {
        switch (direction)
        {
            case RoomDirection.Top:
                return roomAbove == null && doorTop != null && !doorTop.isConnected;
            case RoomDirection.Bottom:
                return roomBelow == null && doorBottom != null && !doorBottom.isConnected;
            case RoomDirection.Left:
                return roomLeft == null && doorLeft != null && !doorLeft.isConnected;
            case RoomDirection.Right:
                return roomRight == null && doorRight != null && !doorRight.isConnected;
            default:
                return false;
        }
    }

    // Disable doors that lead to nowhere
    public void DisableUnusedDoors()
    {
        if (roomAbove == null && doorTop != null) doorTop.gameObject.SetActive(false);
        if (roomBelow == null && doorBottom != null) doorBottom.gameObject.SetActive(false);
        if (roomLeft == null && doorLeft != null) doorLeft.gameObject.SetActive(false);
        if (roomRight == null && doorRight != null) doorRight.gameObject.SetActive(false);
    }
}


public enum RoomDirection
{
    None,
    Top,
    Bottom,
    Left,
    Right
}

