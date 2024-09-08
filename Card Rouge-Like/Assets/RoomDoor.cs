using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    public bool isConnected = false; // To check if the door is connected to another room
    public float doorOffsetX = 10f;  // Horizontal offset to place the next room
    public float doorOffsetY = 10f;  // Vertical offset to place the next room

    [Header("1 = top, 2 = right = 3 = bottom, 4 = left")]
    public int doorPosition; // 1 = top, 2 = right = 3 = bottom, 4 = left;

    public Transform playerTpPos;

    public Room parentRoom;
    public Room connectedRoom;


    // Check if player enters the room
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (doorPosition)
            {
                case 1: //top
                    parentRoom.player.transform.position = connectedRoom.doorBottom.playerTpPos.position;
                    break;
                case 2: //right
                    parentRoom.player.transform.position = connectedRoom.doorLeft.playerTpPos.position;
                    break;
                case 3: //bottom
                    parentRoom.player.transform.position = connectedRoom.doorTop.playerTpPos.position;
                    break;
                case 4: // left 
                    parentRoom.player.transform.position = connectedRoom.doorRight.playerTpPos.position;
                    break;
            }
        }
    }
}
